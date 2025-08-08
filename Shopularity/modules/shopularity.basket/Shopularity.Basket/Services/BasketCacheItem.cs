using System.Collections.Generic;

namespace Shopularity.Basket.Services;

public class BasketCacheItem
{
    public List<BasketItem> Items { get; set; } = new List<BasketItem>();
}