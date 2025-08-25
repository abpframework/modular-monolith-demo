using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Shopularity.Basket.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace Shopularity.Basket.Domain;

public class BasketManager: DomainService
{
    private readonly IDistributedEventBus _eventBus;
    private readonly IDistributedCache<BasketCacheItem> _cache;

    public BasketManager(IDistributedEventBus eventBus, IDistributedCache<BasketCacheItem> cache)
    {
        _eventBus = eventBus;
        _cache = cache;
    }
    

    public async Task RemoveItemsFromUserBasketAsync(Guid userId, List<BasketItem> items)
    {
        var basket = await GetBasketAsync(userId);
        
        foreach (var item in items)
        {
            if (basket.Items.All(x => x.ItemId != item.ItemId))
            {
                continue;
            }

            var itemInCache = basket.Items.First(x => x.ItemId == item.ItemId);
            itemInCache.Amount -= item.Amount;

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
        
        await _eventBus.PublishAsync(
            new BasketUpdatedEto
            {
                UserId = userId,
                ItemCountInBasket = basket.Items.Count
            });
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
}