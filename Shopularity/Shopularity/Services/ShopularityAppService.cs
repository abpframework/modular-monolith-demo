using Microsoft.AspNetCore.Authorization;
using Shopularity.Ordering.Orders;
using Shopularity.Ordering.Orders.Public;
using Shopularity.Services.Dtos;
using Shopularity.Services.Orders;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace Shopularity.Services;

[Authorize]
[RemoteService]
public class ShopularityAppService: ShopularityAppServiceBase, IShopularityAppService
{
    private readonly OrderManager _orderManager;
    private readonly IOrdersAppService _ordersAppAdminService;
    private readonly IOrdersPublicAppService _ordersPublicAppService;

    public ShopularityAppService(
        OrderManager orderManager,
        IOrdersAppService ordersAppAdminService,
        IOrdersPublicAppService ordersPublicAppService
    )
    {
        _orderManager = orderManager;
        _ordersAppAdminService = ordersAppAdminService;
        _ordersPublicAppService = ordersPublicAppService;
    }
    
    public async Task CreateOrderAsync(NewOrderInputDto input)
    {
        if (input.Products.Count == 0)
        {
            //todo: make business exception
            throw new UserFriendlyException("Order should contain a product!");
        }

        await _ordersPublicAppService.CreateAsync(new OrderCreatePublicDto
        {
            ShippingAddress = input.Address,
            Products = input.Products.Select(x=> new OrderCreatePublicProductDto{ProductId = Guid.Parse(x.ItemId), Amount = x.Amount}).ToList(),
        });
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