using Shopularity.Catalog.Services.Categories.Public;

namespace Shopularity.Catalog.Services.Products.Public;

public class ProductWithNavigationPropertiesPublicDto
{
    public ProductPublicDto Product { get; set; } = null!;

    public CategoryPublicDto Category { get; set; } = null!;
}