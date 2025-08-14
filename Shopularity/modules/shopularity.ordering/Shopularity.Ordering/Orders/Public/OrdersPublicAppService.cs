using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Users;

namespace Shopularity.Ordering.Orders.Public;

[Authorize]
public class OrdersPublicAppService: OrderingAppService, IOrdersPublicAppService
{
    private readonly OrderManager _orderManager;

    public OrdersPublicAppService(OrderManager orderManager)
    {
        _orderManager = orderManager;
    }
    
    public virtual async Task<OrderDto> CreateAsync(OrderCreatePublicDto input)
    {
        var order = await _orderManager.CreateAsync(
            CurrentUser.GetId().ToString(),
            OrderState.New,
            input.TotalPrice,
            input.ShippingAddress
        );

        return ObjectMapper.Map<Order, OrderDto>(order);
    }
}