using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace Shopularity.Basket.Services;

[Authorize]
public class BasketAppService : BasketAppServiceBase, IBasketAppService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedEventBus _eventBus;

    public BasketAppService(IMemoryCache memoryCache, IDistributedEventBus eventBus)
    {
        _memoryCache = memoryCache;
        _eventBus = eventBus;
    }

    public async Task AddItemToBasketAsync(BasketItem input)
    {
        _memoryCache.TryGetValue(CurrentUser.GetId(), out BasketCacheItem? value);

        if (value == null)
        {
            value = new BasketCacheItem();
            value.Items.Add(input);
        }
        else if (value.Items.Any(x => x.ItemId == input.ItemId))
        {
            value.Items.First(x => x.ItemId == input.ItemId).Amount += input.Amount;
        }
        else
        {
            value.Items.Add(input);
        }

        _memoryCache.Set(CurrentUser.GetId(), value);

        await _eventBus.PublishAsync(
            new BasketUpdatedEto
            {
                UserId = CurrentUser.GetId(),
                ItemCountInBasket = value.Items.Count
            }
        );
    }

    public async Task RemoveItemFromBasketAsync(BasketItem input)
    {
        _memoryCache.TryGetValue(CurrentUser.GetId(), out BasketCacheItem? value);

        if (value == null)
        {
            return;
        }

        if (value.Items.All(x => x.ItemId != input.ItemId))
        {
            return;
        }

        var item = value.Items.First(x => x.ItemId == input.ItemId);
        item.Amount -= input.Amount;

        if (item.Amount <= 0)
        {
            value.Items.Remove(item);
        }

        _memoryCache.Set(CurrentUser.GetId(), value);

        await _eventBus.PublishAsync(
            new BasketUpdatedEto
            {
                UserId = CurrentUser.GetId(),
                ItemCountInBasket = value.Items.Count
            }
        );
    }

    public async Task<List<BasketItem>> GetBasketItems()
    {
        _memoryCache.TryGetValue(CurrentUser.GetId(), out BasketCacheItem? value);

        return (value ?? new BasketCacheItem()).Items;
    }
}