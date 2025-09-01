using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Shopularity.Basket.Events;
using Shopularity.Basket.Services;
using Shopularity.Basket.SignalR;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Local;

namespace Shopularity.Basket.Domain;

public class BasketManager: DomainService
{
    private readonly IDistributedCache<BasketCacheItem> _cache;
    private readonly ILocalEventBus _eventBus;

    public BasketManager(
        IDistributedCache<BasketCacheItem> cache, 
        ILocalEventBus eventBus)
    {
        _cache = cache;
        _eventBus = eventBus;
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
            //ignored
        }
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