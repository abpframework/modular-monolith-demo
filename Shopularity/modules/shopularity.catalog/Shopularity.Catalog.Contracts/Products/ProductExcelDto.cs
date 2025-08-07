namespace Shopularity.Catalog.Products;

public class ProductExcelDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public double Price { get; set; }
    public int StockCount { get; set; }
}