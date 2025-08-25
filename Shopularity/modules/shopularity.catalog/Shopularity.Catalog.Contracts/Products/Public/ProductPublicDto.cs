using System;

namespace Shopularity.Catalog.Products.Public;

public class ProductPublicDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
        
    public string? Description { get; set; }
        
    public double Price { get; set; }
        
    public int StockCount { get; set; }
        
    public Guid? CategoryId { get; set; }
}