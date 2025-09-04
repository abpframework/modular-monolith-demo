using Shopularity.Catalog.Products.Public;

namespace Shopularity.Basket.Services;

public class BasketItemDto
{
    public ProductPublicDto Product { get; set; }
    
    public int Amount { get; set; }
}