using Microsoft.AspNetCore.Authorization;
using Shopularity.Ordering.Orders;
using Shopularity.Services.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace Shopularity.Services.Orders;

[Authorize]
[RemoteService]
public class ShopularityAppService: ShopularityAppServiceBase, IShopularityAppService
{
    private readonly OrderManager _orderManager;
    private readonly IOrdersAppService _ordersAppAdminService;

    public ShopularityAppService(
        OrderManager orderManager,
        IOrdersAppService ordersAppAdminService
    )
    {
        _orderManager = orderManager;
        _ordersAppAdminService = ordersAppAdminService;
    }
    
    public async Task CreateOrderAsync(NewOrderInputDto input)
    {
        if (input.Products.Count == 0)
        {
            //todo: make business exception
            throw new UserFriendlyException("Order should contain a product!");
        }

        var products = input.Products.Select(x => new KeyValuePair<string, int>(x.ItemId, x.Amount)).ToDictionary();
        await _orderManager.CreateNewAsync(CurrentUser.GetId().ToString(), input.Address, products);
    }
    
    public async Task CancelOrderAsync(Guid id)
    {
        await _orderManager.CancelAsync(id);
    }

    public async Task<PagedResultDto<OrderDto>> GetOrdersAsync()
    {
        var result = await _ordersAppAdminService.GetListAsync(new GetOrdersInput
        {
            MaxResultCount = 1000,
            UserId = CurrentUser.GetId().ToString()
        });

        return result;
    }
}