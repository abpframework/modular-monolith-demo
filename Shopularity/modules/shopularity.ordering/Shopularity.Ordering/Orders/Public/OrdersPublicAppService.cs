using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Shopularity.Catalog.Products;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace Shopularity.Ordering.Orders.Public;

[Authorize]
public class OrdersPublicAppService: OrderingAppService, IOrdersPublicAppService
{
    private readonly OrderManager _orderManager;
    private readonly IProductsIntegrationService _productsIntegrationService;
    private readonly IOrderRepository _orderRepository;

    public OrdersPublicAppService(
        OrderManager orderManager,
        IProductsIntegrationService productsIntegrationService,
        IOrderRepository orderRepository)
    {
        _orderManager = orderManager;
        _productsIntegrationService = productsIntegrationService;
        _orderRepository = orderRepository;
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

    public async Task<ListResultDto<OrderPublicDto>> GetListAsync()
    {
        var items = await (await _orderRepository.GetQueryableAsync()).Where(x=> x.UserId == CurrentUser.GetId()).ToListAsync();
        
        return new ListResultDto<OrderPublicDto>
        {
            Items = ObjectMapper.Map<List<Order>, List<OrderPublicDto>>(items)
        };
    }
}