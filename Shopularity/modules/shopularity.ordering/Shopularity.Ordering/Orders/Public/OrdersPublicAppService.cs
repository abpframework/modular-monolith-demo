using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Shopularity.Catalog.Products;
using Shopularity.Payment.Payments.Events;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace Shopularity.Ordering.Orders.Public;

[Authorize]
public class OrdersPublicAppService: OrderingAppService, IOrdersPublicAppService
{
    private readonly OrderManager _orderManager;
    private readonly IProductsIntegrationService _productsIntegrationService;
    private readonly IDistributedEventBus _eventBus;
    private readonly IOrderRepository _orderRepository;

    public OrdersPublicAppService(
        OrderManager orderManager,
        IProductsIntegrationService productsIntegrationService,
        IDistributedEventBus eventBus,
        IOrderRepository orderRepository)
    {
        _orderManager = orderManager;
        _productsIntegrationService = productsIntegrationService;
        _eventBus = eventBus;
        _orderRepository = orderRepository;
    }
    
    public virtual async Task<OrderDto> CreateAsync(OrderCreatePublicDto input)
    {
        if (input.Products.Count == 0)
        {
            throw new BusinessException(OrderingErrorCodes.OrderShouldContainProducts);
        }
        
        // TODO: GetProductsWithAmountControlAsync is unnecessary
        var products = await _productsIntegrationService
            .GetProductsWithAmountControlAsync(input.Products);
        
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
    
    public async Task CancelAsync(Guid id)
    {
        var order = await _orderRepository.GetAsync(id);
        
        if (order.UserId != CurrentUser.GetId())
        {
            throw new BusinessException(OrderingErrorCodes.CanOnlyCancelOwnedOrders);
        }
        
        if (order.State.IsShipped())
        {
            throw new BusinessException(OrderingErrorCodes.CanOnlyCancelNotShippedOrders);
        }

        order.State = OrderState.Cancelled;

        await _orderRepository.UpdateAsync(order);

        await _eventBus.PublishAsync(new OrderCancelledEto
        {
            OrderId = order.Id
        });
    }

    public async Task<ListResultDto<OrderPublicDto>> GetListAsync()
    {
        var items = await (await _orderRepository.GetQueryableAsync())
            .Where(x=> x.UserId == CurrentUser.GetId())
            .Include(x=> x.OrderLines)
            .ToListAsync();
        
        return new ListResultDto<OrderPublicDto>
        {
            Items = ObjectMapper.Map<List<Order>, List<OrderPublicDto>>(items)
        };
    }
}