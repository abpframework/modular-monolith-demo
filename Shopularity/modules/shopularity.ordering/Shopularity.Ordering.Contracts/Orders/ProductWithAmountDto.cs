using Shopularity.Catalog.Products.Admin;

namespace Shopularity.Ordering.Orders;

public class ProductWithAmountDto
{
    public ProductDto Product { get; set; }
    
    public int Amount { get; set; }
}