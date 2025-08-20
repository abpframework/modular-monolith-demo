using System;

namespace Shopularity.Catalog.Products;

public class ProductStockDecreaseEto
{
    public Guid ProductId { get; set; }
    
    public int Amount { get; set; }
}