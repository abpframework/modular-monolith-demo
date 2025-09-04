using System.Collections.Generic;
using Shopularity.Basket.Services;

namespace Shopularity.Basket.Domain;

public class BasketCacheItem
{
    public List<BasketItem> Items { get; set; } = new();
}