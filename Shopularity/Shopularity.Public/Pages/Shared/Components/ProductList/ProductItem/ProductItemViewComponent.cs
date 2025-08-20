using Microsoft.AspNetCore.Mvc;
using Shopularity.Catalog.Products.Public;
using Shopularity.Public.Components.Basket;
using Shopularity.Public.Pages.Shared.Components.Basket.BasketItem;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Public.Pages.Shared.Components.ProductList.ProductItem;

public class ProductItemViewModel
{
    public ProductWithNavigationPropertiesPublicDto Item { get; set; }
    
    public bool ShownSolo { get; set; }

    public ProductItemViewModel(ProductWithNavigationPropertiesPublicDto item)
    {
        Item = item;
    }
}

public class ProductItemViewComponent : AbpViewComponent
{
    public IViewComponentResult Invoke(ProductItemViewModel model)
    {
        return View("~/Pages/Shared/Components/ProductList/ProductItem/ProductItem.cshtml", model);
    }
}
