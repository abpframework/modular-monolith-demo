using Microsoft.AspNetCore.Mvc;
using Shopularity.Catalog.Services.Products.Public;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Public.Pages.Shared.Components.ProductList.ProductItem;

public class ProductItemViewModel
{
    public ProductWithNavigationPropertiesPublicDto Item { get; set; }

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
