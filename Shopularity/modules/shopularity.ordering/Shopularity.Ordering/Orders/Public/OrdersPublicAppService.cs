using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Shopularity.Catalog.Products;
using Shopularity.Catalog.Products.Admin;
using Volo.Abp.Users;

namespace Shopularity.Ordering.Orders.Public;

[Authorize]
public class OrdersPublicAppService: OrderingAppService, IOrdersPublicAppService
{
    private readonly OrderManager _orderManager;
    private readonly IProductsIntegrationService _productsIntegrationService;

    public OrdersPublicAppService(OrderManager orderManager, IProductsIntegrationService productsIntegrationService)
    {
        _orderManager = orderManager;
        _productsIntegrationService = productsIntegrationService;
    }
    
    public virtual async Task<OrderDto> CreateAsync(OrderCreatePublicDto input)
    {
        var products = await _productsIntegrationService.GetProductsWithAmountControlAsync(input.Products);
        
        var order = await _orderManager.CreateNewAsync(
            CurrentUser.GetId(),
            input.ShippingAddress,
            products.Select(x=> new ProductWithAmountDto
            {
                Product = x,
                Amount = input.Products.First(y=>  x.Id == y.ProductId).Amount
            }).ToList()
        );

        return ObjectMapper.Map<Order, OrderDto>(order);
    }
}