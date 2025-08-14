using Microsoft.AspNetCore.Mvc;
using Shopularity.Catalog.Products.Public;
using Shopularity.Public.Components.Basket;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Public.Pages.Shared.Components.Basket.BasketItem;

public class BasketItemViewModel
{
    public BasketViewItemModel Item { get; set; }

    public BasketItemViewModel(BasketViewItemModel item)
    {
        Item = item;
    }
}

public class BasketItemViewComponent : AbpViewComponent
{
    public IViewComponentResult Invoke(BasketItemViewModel model)
    {
        return View("~/Pages/Shared/Components/Basket/BasketItem/BasketItem.cshtml", model);
    }
}
