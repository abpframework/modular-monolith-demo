using System.Collections.Generic;

namespace Shopularity.Basket.Domain.Basket;

public class BasketCacheItem
{
    public List<BasketItem> Items { get; set; } = new();
}