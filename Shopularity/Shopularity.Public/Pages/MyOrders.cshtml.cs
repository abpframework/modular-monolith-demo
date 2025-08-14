using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Shopularity.Basket.Services;
using Shopularity.Catalog.Products;
using Shopularity.Catalog.Products.Public;
using Shopularity.Public.Components.Basket;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Shopularity.Public.Pages;

public class MyOrdersModel : ShopularityPublicPageModel
{

    public MyOrdersModel()
    {
        
    }
    
    public virtual async Task<ActionResult> OnGetAsync()
    {
        return Page();
    }
}