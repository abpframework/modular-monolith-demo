using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Catalog.Services.Products.Public;

namespace Shopularity.Public.Components.Basket;

public class BasketViewModel
{
    public List<BasketViewItemModel> Items { get; set; }
    
    public double TotalPrice { get; set; }

    public BasketViewModel(List<BasketViewItemModel> items)
    {
        Items = items;
        TotalPrice = items.Select(item => item.Price * item.Amount).Sum();
    }
}

public class BasketViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(BasketViewModel model)
    {
        return View("Default", model);
    }
}

public class BasketViewItemModel : ProductPublicDto
{
    public int Amount { get; set; }
}