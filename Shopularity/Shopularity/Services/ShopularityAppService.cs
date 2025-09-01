using Microsoft.AspNetCore.Authorization;
using Shopularity.Catalog.Products;
using Shopularity.Ordering.Orders;
using Shopularity.Ordering.Orders.Public;
using Shopularity.Payment.Payments;
using Shopularity.Services.Dtos;
using Shopularity.Services.Orders;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Shopularity.Services;

[Authorize]
[RemoteService]
public class ShopularityAppService: ShopularityAppServiceBase, IShopularityAppService
{
    private readonly OrderManager _orderManager;
    private readonly PaymentManager _paymentManager;
    private readonly IOrdersPublicAppService _ordersPublicAppService;

    public ShopularityAppService(
        OrderManager orderManager,
        PaymentManager paymentManager,
        IOrdersPublicAppService ordersPublicAppService
    )
    {
        _orderManager = orderManager;
        _paymentManager = paymentManager;
        _ordersPublicAppService = ordersPublicAppService;
    }
    
    public async Task CreateOrderAsync(NewOrderInputDto input)
    {
        if (input.Products.Count == 0)
        {
            throw new BusinessException(ShopularityErrorCodes.OrderShouldContainProducts);
        }

        var order = await _ordersPublicAppService.CreateAsync(new OrderCreatePublicDto
        {
            ShippingAddress = input.Address,
            Products = input.Products.Select(x=> new ProductIdsWithAmountDto{ProductId = x.ItemId, Amount = x.Amount}).ToList(),
        });
    }
    
    public async Task CancelOrderAsync(Guid id)
    {
        await _orderManager.CancelAsync(id);
    }

    public async Task<ListResultDto<OrderPublicDto>> GetOrdersAsync()
    {
        return await _ordersPublicAppService.GetListAsync();
    }
}