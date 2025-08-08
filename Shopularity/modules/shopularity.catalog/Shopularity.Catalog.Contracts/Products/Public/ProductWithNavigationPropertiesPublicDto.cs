using Shopularity.Catalog.Categories.Public;

namespace Shopularity.Catalog.Products.Public;

public class ProductWithNavigationPropertiesPublicDto
{
    public ProductPublicDto Product { get; set; } = null!;

    public CategoryPublicDto Category { get; set; } = null!;
}