using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Volo.Abp.Users;

namespace Shopularity.Basket.Services;

[Authorize]
public class BasketAppService: BasketAppServiceBase, IBasketAppService
{
    private readonly IMemoryCache _memoryCache;

    public BasketAppService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task AddItemToBasket(BasketItem input)
    {
        _memoryCache.TryGetValue(CurrentUser.GetId(), out BasketCacheItem? value);

        if (value == null)
        {
            value = new BasketCacheItem();
        }
        
        value.Items.Add(input);
        
        _memoryCache.Set(CurrentUser.GetId(), value);
        // todo: publish event
    }

    public async Task<List<BasketItem>> GetBasketItems()
    {
        _memoryCache.TryGetValue(CurrentUser.GetId(), out BasketCacheItem? value);
        
        return (value ?? new BasketCacheItem()).Items;
    }
}