using System;

namespace Shopularity.Catalog.Services.Products.Admin;

[Serializable]
public class ProductDownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}