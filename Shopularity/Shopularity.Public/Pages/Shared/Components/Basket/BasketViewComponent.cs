using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Basket.Services;
using Shopularity.Catalog.Products.Public;

namespace Shopularity.Public.Components.Basket;

public class BasketViewModel
{
    public List<BasketViewItemModel> Items { get; set; }
    
    public double TotalPrice { get; set; }
    
    public bool IsBasketPage { get; set; }

    public BasketViewModel(List<BasketViewItemModel> items, bool isBasketPage = false)
    {
        Items = items;
        TotalPrice = items.Select(item => item.Product.Price * item.Amount).Sum();
        IsBasketPage = isBasketPage;
    }
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