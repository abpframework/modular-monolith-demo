using System;

namespace Shopularity.Ordering.Orders;

[Serializable]
public class OrderDownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}