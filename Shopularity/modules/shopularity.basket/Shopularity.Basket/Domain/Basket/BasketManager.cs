using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Shopularity.Basket.Events;
using Shopularity.Catalog.Services.Products.Integration;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Local;

namespace Shopularity.Basket.Domain.Basket;

public class BasketManager: DomainService
{
    private readonly IDistributedCache<BasketCacheItem> _cache;
    private readonly ILocalEventBus _eventBus;
    private readonly IProductsIntegrationService _productsIntegrationService;

    public BasketManager(
        IDistributedCache<BasketCacheItem> cache, 
        ILocalEventBus eventBus,
        IProductsIntegrationService productsIntegrationService)
    {
        _cache = cache;
        _eventBus = eventBus;
        _productsIntegrationService = productsIntegrationService;
    }

    public async Task AddItemAsync(Guid userId, Guid itemId, int amount = 1)
    {
        var basket = await GetBasketAsync(userId);

        if (basket!.Items.Any(x => x.ItemId == itemId))
        {
            basket.Items.First(x => x.ItemId == itemId).Amount += amount;
        }
        else
        {
            basket.Items.Add(new BasketItem
            {
                Amount = amount,
                ItemId = itemId
            });
        }

        await CheckStockAsync(basket.Items.First(x => x.ItemId == itemId));

        await _cache.SetAsync(userId.ToString(), basket);

        await PublishBasketUpdatedEventAsync(userId, basket);
    }

    public async Task RemoveItemsAsync(Guid userId, Dictionary<Guid, int> items)
    {
        if (items.Count == 0)
        {
            return;
        }
        
        var basket = await GetBasketAsync(userId);
        
        foreach (var item in items)
        {
            var id = item.Key;
            var amount = item.Value;
            
            if (basket.Items.All(x => x.ItemId != id))
            {
                continue;
            }

            var itemInCache = basket.Items.First(x => x.ItemId == id);
            itemInCache.Amount -= amount;

            if (itemInCache.Amount <= 0)
            {
                basket.Items.Remove(itemInCache);
            }
        }

        if (basket.Items.Any())
        {
            await _cache.SetAsync(userId.ToString(), basket);
        }
        else
        {
            await _cache.RemoveAsync(userId.ToString());
        }

        await PublishBasketUpdatedEventAsync(userId, basket);
    }
    
    public async Task<List<BasketItemWithProductInfo>> GetItemsAsync(Guid userId)
    {
        var items = (await GetBasketAsync(userId)).Items;

        var products = await _productsIntegrationService.GetPublicProductsAsync(items.Select(x => x.ItemId).ToList());

        return products.Select(x => new BasketItemWithProductInfo
        {
            Product = x,
            Amount = items.First(y => y.ItemId == x.Id).Amount
        }).ToList();
    }

    public async Task<int> GetCountOfItemsAsync(Guid userId)
    {
        return (await GetBasketAsync(userId)).Items.Count;
    }

    private async Task<BasketCacheItem> GetBasketAsync(Guid userId)
    {
        var basket = await _cache.GetOrAddAsync(
            userId.ToString(),
            () => Task.FromResult(new BasketCacheItem()),
            () => new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMonths(1)
            }
        );
        return basket!;
    }

    private async Task PublishBasketUpdatedEventAsync(Guid userId, BasketCacheItem basket)
    {
        try
        {
            await _eventBus.PublishAsync(new BasketUpdatedEto
            {
                UserId = userId,
                ItemCountInBasket = basket.Items.Count
            });
        }
        catch
        {
            // ignored
        }
    }

    private async Task CheckStockAsync(BasketItem item)
    {
        var isStockEnough = await _productsIntegrationService.CheckStockAsync(item.ItemId, item.Amount);

        if (!isStockEnough)
        {
            throw new BusinessException(BasketErrorCodes.NotEnoughStock);
        }
    }
}