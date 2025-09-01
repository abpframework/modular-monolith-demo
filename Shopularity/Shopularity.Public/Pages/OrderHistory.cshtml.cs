using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Ordering.Orders.Public;
using Shopularity.Services.Orders;
using Volo.Abp.Application.Dtos;

namespace Shopularity.Public.Pages;

public class OrderHistoryModel : ShopularityPublicPageModel
{
    public IShopularityAppService ShopularityAppService { get; }
    
    public ListResultDto<OrderPublicDto> Orders { get; set; }

    public OrderHistoryModel(IShopularityAppService shopularityAppService)
    {
        ShopularityAppService = shopularityAppService;
    }
    
    public virtual async Task<ActionResult> OnGetAsync()
    {
        Orders = await ShopularityAppService.GetOrdersAsync();
        
        return Page();
    }
}