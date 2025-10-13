using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Ordering.Services.Orders.Public;
using Volo.Abp.Application.Dtos;

namespace Shopularity.Public.Pages;

public class OrderHistoryModel : ShopularityPublicPageModel
{
    public IOrdersPublicAppService OrdersPublicAppService { get; }
    public ListResultDto<OrderPublicDto> Orders { get; set; }

    public OrderHistoryModel(IOrdersPublicAppService ordersPublicAppService)
    {
        OrdersPublicAppService = ordersPublicAppService;
    }
    
    public virtual async Task<ActionResult> OnGetAsync()
    {
        Orders = await OrdersPublicAppService.GetListAsync();
        
        return Page();
    }
}