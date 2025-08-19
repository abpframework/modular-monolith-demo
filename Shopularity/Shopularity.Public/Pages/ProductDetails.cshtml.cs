using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Catalog.Products.Public;

namespace Shopularity.Public.Pages;

public class ProductDetailsModel : ShopularityPublicPageModel
{
    public IProductsPublicAppService ProductsPublicAppService { get; }
    
    public ProductWithNavigationPropertiesPublicDto Item { get; set; }

    public ProductDetailsModel(IProductsPublicAppService productsPublicAppService)
    {
        ProductsPublicAppService = productsPublicAppService;
    }
    
    public virtual async Task<ActionResult> OnGetAsync(Guid id)
    {
        Item = await ProductsPublicAppService.GetAsync(id);
        return Page();
    }
}