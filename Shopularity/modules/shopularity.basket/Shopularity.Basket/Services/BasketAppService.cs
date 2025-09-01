using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Shopularity.Basket.SignalR;
using Shopularity.Catalog.Products;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Caching;
using Volo.Abp.Users;

namespace Shopularity.Basket.Services;

[Authorize]
public class BasketAppService : BasketAppServiceBase, IBasketAppService
{
    private readonly IDistributedCache<BasketCacheItem> _cache;
    private readonly IProductsIntegrationService _productsService;
    private readonly IHubContext<BasketHub> _basketHub;

    public BasketAppService(
        IDistributedCache<BasketCacheItem> cache,
        IProductsIntegrationService productsService,
        IHubContext<BasketHub> basketHub)
    {
        _cache = cache;
        _productsService = productsService;
        _basketHub = basketHub;
    }

    public async Task AddItemToBasketAsync(BasketItem input)
    {
        var basket = await GetBasketAsync();

        if (basket!.Items.Any(x => x.ItemId == input.ItemId))
        {
            basket.Items.First(x => x.ItemId == input.ItemId).Amount += input.Amount;
        }
        else
        {
            basket.Items.Add(input);
        }

        await CheckStockAsync(basket.Items.First(x => x.ItemId == input.ItemId));

        await _cache.SetAsync(CurrentUser.GetId().ToString(), basket);

        await _basketHub
            .Clients
            .User(CurrentUser.GetId().ToString())
            .SendAsync(
                "BasketUpdated",
                new BasketUpdatedEto
                {
                    ItemCountInBasket = basket.Items.Count
                }
            );
    }

    public async Task RemoveItemFromBasketAsync(BasketItem input)
    {
        var basket = await GetBasketAsync();

        if (basket.Items.All(x => x.ItemId != input.ItemId))
        {
            return;
        }

        var item = basket.Items.First(x => x.ItemId == input.ItemId);
        item.Amount -= input.Amount;

        if (item.Amount <= 0)
        {
            basket.Items.Remove(item);
        }

        if (basket.Items.Any())
        {
            await _cache.SetAsync(CurrentUser.GetId().ToString(), basket);
        }
        else
        {
            await _cache.RemoveAsync(CurrentUser.GetId().ToString());
        }
        
        await _basketHub
            .Clients
            .User(CurrentUser.GetId().ToString())
            .SendAsync(
                "BasketUpdated",
                new BasketUpdatedEto
                {
                    ItemCountInBasket = basket.Items.Count
                }
            );
    }

    public async Task<ListResultDto<BasketItemDto>> GetBasketItemsAsync()
    {
        var items = (await GetBasketAsync()).Items;

        var products = await _productsService.GetProductsAsync(items.Select(x => x.ItemId).ToList());

        return new ListResultDto<BasketItemDto>(products.Select(x => new BasketItemDto
        {
            Product = x,
            Amount = items.First(y => y.ItemId == x.Id).Amount
        }).ToList());
    }

    public async Task<int> GetCountOfItemsInBasketAsync()
    {
        return (await GetBasketAsync()).Items.Count;
    }

    private async Task<BasketCacheItem> GetBasketAsync()
    {
        var basket = await _cache.GetOrAddAsync(
            CurrentUser.GetId().ToString(),
            () => Task.FromResult(new BasketCacheItem()),
            () => new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMonths(1)
            }
        );
        return basket!;
    }

    private async Task CheckStockAsync(BasketItem item)
    {
        var isStockEnough = await _productsService.CheckStockAsync(item.ItemId, item.Amount);

        if (!isStockEnough)
        {
            throw new BusinessException(BasketErrorCodes.NotEnoughStock);
        }
    }
}