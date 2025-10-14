using System;

namespace Shopularity.Ordering.Services.Orders.Admin;

[Serializable]
public class OrderDownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}