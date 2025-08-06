using Shopularity.Catalog.Categories;

namespace Shopularity.Catalog.Products
{
    public class ProductWithNavigationPropertiesDto
    {
        public ProductDto Product { get; set; } = null!;

        public CategoryDto Category { get; set; } = null!;

    }
}