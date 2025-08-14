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
    private readonly IOrdersAppService _ordersAppAdminService;
    private readonly IOrdersPublicAppService _ordersPublicAppService;
    private readonly IOrderLinesAppService _orderLinesAppService;
    private readonly IProductsPublicAppService _productsPublicAppService;
    private readonly IBasketAppService _basketAppService;
    private readonly PaymentManager _paymentManager;

    public OrderingPublicAppService(
        IOrdersAppService ordersAppAdminService,
        IOrdersPublicAppService ordersPublicAppService,
        IOrderLinesAppService orderLinesAppService,
        IProductsPublicAppService productsPublicAppService,
        IBasketAppService basketAppService,
        PaymentManager paymentManager
    )
    {
        _ordersAppAdminService = ordersAppAdminService;
        _ordersPublicAppService = ordersPublicAppService;
        _orderLinesAppService = orderLinesAppService;
        _productsPublicAppService = productsPublicAppService;
        _basketAppService = basketAppService;
        _paymentManager = paymentManager;
    }
    
    public async Task CreateOrderAsync(NewOrderInputDto input)
    {
        //todo: remove appservice to appservice usage. create a manager class or use repository etc.
        
        var products = await _productsPublicAppService.GetListByIdsAsync(new GetListByIdsInput
        {
            Ids = input.Products.Select(x => x.ProductId).ToList()
        });

        await CheckProductStocksAsync(input, products);
        var totalPrice = await CalculateTotalPricesAsync(input, products);

        var order = await _ordersPublicAppService.CreateAsync(new OrderCreatePublicDto
        {
            ShippingAddress = input.Address,
            TotalPrice = totalPrice
        });

        foreach (var item in products.Items)
        {
            await _orderLinesAppService.CreateAsync(new OrderLineCreateDto
            {
                OrderId = order.Id,
                ProductId = item.Product.Id.ToString(),
                Name = item.Product.Name,
                Price = item.Product.Price,
                Amount = input.Products.First(x => x.ProductId == item.Product.Id).Amount,
                TotalPrice = item.Product.Price * input.Products.First(x => x.ProductId == item.Product.Id).Amount
            });
        }

        await _paymentManager.CreateAsync(order.Id.ToString());

        foreach (var product in input.Products)
        {
            await _basketAppService.RemoveItemFromBasket(product);
        }
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

    private async Task<double> CalculateTotalPricesAsync(NewOrderInputDto input, ListResultDto<ProductWithNavigationPropertiesPublicDto> products)
    {
        double result = 0;
        foreach (var item in products.Items)
        {
            var itemOrdered = input.Products.First(x => x.ProductId == item.Product.Id);

            result += itemOrdered.Amount * item.Product.Price;
        }

        return result;
    }

    private async Task CheckProductStocksAsync(NewOrderInputDto input, ListResultDto<ProductWithNavigationPropertiesPublicDto> products)
    {
        foreach (var item in products.Items)
        {
            var itemOrdered = input.Products.First(x => x.ProductId == item.Product.Id);

            if (item.Product.StockCount < itemOrdered.Amount)
            {
                //todo: show better message with code
                throw new BusinessException(message:"Not Enough stock!!");
            }
        }
    }
}