using System;

namespace Shopularity.Catalog.Products;

public class ProductIdsWithAmountDto
{
    public Guid ProductId { get; set; }
    
    public int Amount { get; set; }
}