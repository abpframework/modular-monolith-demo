using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Shopularity.Basket.Services;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace Shopularity.Basket.Domain;

public class BasketManager: DomainService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedEventBus _eventBus;

    public BasketManager(IMemoryCache memoryCache, IDistributedEventBus eventBus)
    {
        _memoryCache = memoryCache;
        _eventBus = eventBus;
    }
    

    public async Task RemoveItemsFromUserBasketAsync(Guid userId, List<BasketItem> items)
    {
        _memoryCache.TryGetValue(userId, out BasketCacheItem? value);
        
        if (value == null)
        {
            return;
        }
        
        foreach (var item in items)
        {
            if (value.Items.All(x => x.ItemId != item.ItemId))
            {
                continue;
            }

            var itemInCache = value.Items.First(x => x.ItemId == item.ItemId);
            itemInCache.Amount -= item.Amount;

            if (itemInCache.Amount <= 0)
            {
                value.Items.Remove(itemInCache);
            }
        }

        _memoryCache.Set(userId, value);
        
        await _eventBus.PublishAsync(
            new BasketUpdatedEto
            {
                UserId = userId,
                ItemCountInBasket = value.Items.Count
            });
    }
    
}