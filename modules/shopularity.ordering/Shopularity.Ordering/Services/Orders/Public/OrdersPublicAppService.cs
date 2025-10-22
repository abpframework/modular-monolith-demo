using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Shopularity.Catalog.Services.Products.Integration;
using Shopularity.Ordering.Domain.Orders;
using Shopularity.Ordering.Domain.Orders.OrderLines;
using Shopularity.Ordering.Events.Orders;
using Shopularity.Ordering.Services.Orders.Admin;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace Shopularity.Ordering.Services.Orders.Public;

[Authorize]
public class OrdersPublicAppService: OrderingAppService, IOrdersPublicAppService
{
    private readonly IProductsIntegrationService _productsIntegrationService;
    private readonly IDistributedEventBus _eventBus;
    private readonly IOrderRepository _orderRepository;

    public OrdersPublicAppService(
        IProductsIntegrationService productsIntegrationService,
        IDistributedEventBus eventBus,
        IOrderRepository orderRepository)
    {
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
            .GetPublicProductsAsync(input.Products.Select(x=> x.ProductId).ToList());

        var productsOutOfStock = products.Where(product => input.Products.First(y => product.Id == y.ProductId).Amount > product.StockCount).ToList();
        if (productsOutOfStock.Count != 0)
        {
            throw new BusinessException(OrderingErrorCodes.NotEnoughStock)
                .WithData("ProductNames", productsOutOfStock.Select(x=> x.Name).JoinAsString(", "));
        }

        var productsWithAmounts = products.Select(x => new ProductWithAmountDto
        {
            Product = x,
            Amount = input.Products.First(y => x.Id == y.ProductId).Amount
        }).ToList();

        var order = new Order(
            GuidGenerator.Create(),
            CurrentUser.GetId(),
            productsWithAmounts.Select(x => x.Product.Price * x.Amount).Sum(),
            input.ShippingAddress
        );
            
        foreach (var product in productsWithAmounts)
        {
            var orderLine = new OrderLine(
                GuidGenerator.Create(),
                order.Id,
                product.Product.Id.ToString(),
                product.Product.Price,
                product.Amount,
                product.Amount * product.Product.Price,
                product.Product.Name
            );
                
            order.AddOrderLine(orderLine);
        }
        
        await _orderRepository.InsertAsync(order);

        await _eventBus.PublishAsync(
            new OrderCreatedEto
            {
                Id = order.Id,
                UserId = CurrentUser.GetId(),
                ProductsWithAmounts = productsWithAmounts.ToDictionary(
                    x => x.Product.Id, 
                    x => x.Amount
                ),
                TotalPrice = order.TotalPrice
            }
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

        order.SetState(OrderState.Cancelled);

        await _orderRepository.UpdateAsync(order);
        
        await _eventBus.PublishAsync(new OrderCancelledEto
        {
            OrderId = order.Id
        });
    }

    public async Task<ListResultDto<OrderPublicDto>> GetListAsync()
    {
        var currentUserId = CurrentUser.GetId();
        var items = await (await _orderRepository.GetQueryableAsync())
            .Where(x=> x.UserId == currentUserId)
            .OrderByDescending(x=> x.CreationTime)
            .Include(x=> x.OrderLines)
            .ToListAsync();
        
        return new ListResultDto<OrderPublicDto>
        {
            Items = ObjectMapper.Map<List<Order>, List<OrderPublicDto>>(items)
        };
    }
}