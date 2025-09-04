using Shopularity.Catalog.Products.Public;

namespace Shopularity.Basket.Domain;

public class BasketItemWithProductInfo
{
    public ProductPublicDto Product { get; set; }
    
    public int Amount { get; set; }
}