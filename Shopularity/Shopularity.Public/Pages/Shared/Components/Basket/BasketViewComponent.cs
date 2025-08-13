using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Basket.Services;
using Shopularity.Catalog.Products.Public;

namespace Shopularity.Public.Components.Basket;

public class BasketViewModel
{
    public List<BasketViewItemModel> Items { get; set; } = new();
}

public class BasketViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(BasketViewModel model)
    {
        return View("Default", model);
    }
}

public class BasketViewItemModel : ProductWithNavigationPropertiesPublicDto
{
    public int Amount { get; set; }
}