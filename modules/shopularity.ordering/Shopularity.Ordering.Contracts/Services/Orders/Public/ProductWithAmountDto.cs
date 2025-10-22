using Shopularity.Catalog.Services.Products.Admin;
using Shopularity.Catalog.Services.Products.Public;

namespace Shopularity.Ordering.Services.Orders.Public;

public class ProductWithAmountDto
{
    public ProductPublicDto Product { get; set; }
    
    public int Amount { get; set; }
}