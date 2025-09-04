using Shopularity.Catalog.Services.Categories.Admin;

namespace Shopularity.Catalog.Services.Products.Admin;

public class ProductWithNavigationPropertiesDto
{
    public ProductDto Product { get; set; } = null!;

    public CategoryDto Category { get; set; } = null!;

}