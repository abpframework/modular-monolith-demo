using System.Collections.Generic;

namespace Shopularity.Basket.Domain;

public class BasketCacheItem
{
    public List<BasketItem> Items { get; set; } = new();
}