using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Shopularity.Basket.Services;
using Shopularity.Catalog.Products;
using Shopularity.Catalog.Products.Public;
using Shopularity.Ordering.Orders;
using Shopularity.Public.Components.Basket;
using Shopularity.Services.Orders;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Shopularity.Public.Pages;

public class MyOrdersModel : ShopularityPublicPageModel
{
    public IOrderingPublicAppService OrderingPublicAppService { get; }
    
    public PagedResultDto<OrderDto> Orders { get; set; }

    public MyOrdersModel(IOrderingPublicAppService orderingPublicAppService)
    {
        OrderingPublicAppService = orderingPublicAppService;
    }
    
    public virtual async Task<ActionResult> OnGetAsync()
    {
        Orders = await OrderingPublicAppService.GetOrdersAsync();
        
        return Page();
    }
}