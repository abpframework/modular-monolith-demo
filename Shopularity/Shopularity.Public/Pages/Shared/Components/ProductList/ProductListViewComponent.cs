using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Catalog.Products.Public;

namespace Shopularity.Public.Components.ProductList;

public class ProductListViewModel
{
    public long TotalCount { get; set; }
    public IReadOnlyList<ProductWithNavigationPropertiesPublicDto> Items { get; set; }
}

public class ProductListViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ProductListViewModel model)
    {
        return View("Default", model);
    }
}