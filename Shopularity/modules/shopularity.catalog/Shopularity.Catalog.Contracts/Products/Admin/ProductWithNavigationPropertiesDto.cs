using Shopularity.Catalog.Categories;

namespace Shopularity.Catalog.Products.Admin;

public class ProductWithNavigationPropertiesDto
{
    public ProductDto Product { get; set; } = null!;

    public CategoryDto Category { get; set; } = null!;

}