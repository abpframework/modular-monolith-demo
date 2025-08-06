using System;

namespace Shopularity.Catalog.Products;

[Serializable]
public class ProductDownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}