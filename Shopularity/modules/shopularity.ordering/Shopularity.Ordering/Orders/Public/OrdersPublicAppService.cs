using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Shopularity.Catalog.Products;
using Shopularity.Ordering.Orders.Events;
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
        if (input.Products.IsNullOrEmpty())
        {
            throw new BusinessException(OrderingErrorCodes.OrderShouldContainProducts);
        }
        
        var products = await _productsIntegrationService
            .GetProductsAsync(input.Products);

        if (products.Any(product => input.Products.First(y=> product.Id == y.ProductId).Amount > product.StockCount))
        {
            //TODO: Add product info to the exception
            throw new BusinessException(OrderingErrorCodes.NotEnoughStock);
        }

        var productsWithAmounts = products.Select(x => new ProductWithAmountDto
        {
            Product = x,
            Amount = input.Products.First(y => x.Id == y.ProductId).Amount
        }).ToList();
        
        var order = await _orderManager.CreateNewAsync(
            CurrentUser.GetId(),
            input.ShippingAddress,
            productsWithAmounts
        );
            
        await _eventBus.PublishAsync(new OrderCreatedEto
        {
            Id = order.Id,
            UserId = CurrentUser.GetId(),
            ProductsWithAmounts = productsWithAmounts.ToDictionary(x=> x.Product.Id, x=> x.Amount),
            TotalPrice = order.TotalPrice
        });

        return ObjectMapper.Map<Order, OrderDto>(order);
    }
    
    public async Task CancelAsync(Guid id)
    {
        var order = await _orderRepository.GetAsync(id);
        
        if (order.UserId != CurrentUser.GetId())
        {
            throw new BusinessException(OrderingErrorCodes.CanOnlyCancelOwnedOrders);
        }

        order.Cancel();

        //TODO: update order
        
        await _eventBus.PublishAsync(new OrderCancelledEto
        {
            OrderId = order.Id
        });
    }

    public async Task<ListResultDto<OrderPublicDto>> GetListAsync()
    {
        var items = await (await _orderRepository.GetQueryableAsync())
            .Where(x=> x.UserId == CurrentUser.GetId())
            .OrderByDescending(x=> x.CreationTime)
            .Include(x=> x.OrderLines)
            .ToListAsync();
        
        return new ListResultDto<OrderPublicDto>
        {
            Items = ObjectMapper.Map<List<Order>, List<OrderPublicDto>>(items)
        };
    }
}