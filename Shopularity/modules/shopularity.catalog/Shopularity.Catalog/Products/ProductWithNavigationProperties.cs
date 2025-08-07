using Shopularity.Catalog.Categories;

namespace Shopularity.Catalog.Products;

public  class ProductWithNavigationProperties
{
    public Product Product { get; set; } = null!;

    public Category Category { get; set; } = null!;
}