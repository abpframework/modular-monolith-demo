using Shopularity.Catalog.Services.Products.Admin;

namespace Shopularity.Ordering.Services.Orders.Public;

public class ProductWithAmountDto
{
    public ProductDto Product { get; set; }
    
    public int Amount { get; set; }
}