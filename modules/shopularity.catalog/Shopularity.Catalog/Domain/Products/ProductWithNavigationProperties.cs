using Shopularity.Catalog.Domain.Categories;

namespace Shopularity.Catalog.Domain.Products;

public  class ProductWithNavigationProperties
{
    public Product Product { get; set; } = null!;

    public Category Category { get; set; } = null!;
}