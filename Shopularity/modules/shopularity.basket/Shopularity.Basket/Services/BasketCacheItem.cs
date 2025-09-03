using System.Collections.Generic;

namespace Shopularity.Basket.Services;

public class BasketCacheItem //TODO: Move to Domain
{
    public List<BasketItem> Items { get; set; } = new();
}