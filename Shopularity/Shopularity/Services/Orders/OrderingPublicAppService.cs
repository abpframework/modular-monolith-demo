using Microsoft.AspNetCore.Authorization;
using Shopularity.Basket.Services;
using Shopularity.Catalog.Products;
using Shopularity.Catalog.Products.Admin;
using Shopularity.Catalog.Products.Public;
using Shopularity.Ordering.OrderLines;
using Shopularity.Ordering.Orders;
using Shopularity.Ordering.Orders.Public;
using Shopularity.Payment.Payments;
using Shopularity.Services.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace Shopularity.Services.Orders;

[Authorize]
[RemoteService]
public class OrderingPublicAppService: ShopularityAppServiceBase, IOrderingPublicAppService
{
    private readonly OrderManager _orderManager;
    private readonly IOrdersAppService _ordersAppAdminService;
    private readonly IOrdersPublicAppService _ordersPublicAppService;
    private readonly IOrderLinesAppService _orderLinesAppService;
    private readonly IProductsPublicAppService _productsPublicAppService;
    private readonly IBasketAppService _basketAppService;
    private readonly PaymentManager _paymentManager;

    public OrderingPublicAppService(
        OrderManager orderManager,
        IOrdersAppService ordersAppAdminService,
        IOrdersPublicAppService ordersPublicAppService,
        IOrderLinesAppService orderLinesAppService,
        IProductsPublicAppService productsPublicAppService,
        IBasketAppService basketAppService,
        PaymentManager paymentManager
    )
    {
        _orderManager = orderManager;
        _ordersAppAdminService = ordersAppAdminService;
        _ordersPublicAppService = ordersPublicAppService;
        _orderLinesAppService = orderLinesAppService;
        _productsPublicAppService = productsPublicAppService;
        _basketAppService = basketAppService;
        _paymentManager = paymentManager;
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