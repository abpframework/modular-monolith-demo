using System;

namespace Shopularity.Catalog.Products.Admin;

public class ProductExcelDownloadDto
{
    public string DownloadToken { get; set; } = null!;

    public string? FilterText { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }
    public double? PriceMin { get; set; }
    public double? PriceMax { get; set; }
    public int? StockCountMin { get; set; }
    public int? StockCountMax { get; set; }
    public Guid? CategoryId { get; set; }

    public ProductExcelDownloadDto()
    {

    }
}