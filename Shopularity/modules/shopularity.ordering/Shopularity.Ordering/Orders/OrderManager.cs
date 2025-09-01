using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopularity.Catalog.Products;
using Shopularity.Ordering.OrderLines;
using Shopularity.Ordering.Orders.Events;
using Shopularity.Payment.Payments.Events;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace Shopularity.Ordering.Orders;

public class OrderManager : DomainService
{
    protected IOrderRepository _orderRepository;
    private readonly IOrderLineRepository _orderLineRepository;
    private readonly IDistributedEventBus _eventBus;
    private readonly ICurrentUser _currentUser;

    public OrderManager(IOrderRepository orderRepository,
        IOrderLineRepository orderLineRepository,
        IDistributedEventBus eventBus,
        ICurrentUser currentUser)
    {
        _orderRepository = orderRepository;
        _orderLineRepository = orderLineRepository;
        _eventBus = eventBus;
        _currentUser = currentUser;
    }

    public virtual async Task<Order> CreateNewAsync(
        Guid userId,
        string shippingAddress,
        List<ProductWithAmountDto> productDtos)
    {
        Check.NotNull(userId, nameof(userId));
        Check.NotNullOrWhiteSpace(shippingAddress, nameof(shippingAddress));
        Check.Length(shippingAddress, nameof(shippingAddress), OrderConsts.ShippingAddressMaxLength);

        var order = new Order(
            GuidGenerator.Create(),
            userId,
            OrderState.New,
            productDtos.Select(x=> x.Product.Price * x.Amount).Sum(),
            shippingAddress
        );

        order = await _orderRepository.InsertAsync(order);

        foreach (var product in productDtos)
        {
            var orderLine = new OrderLine(
                GuidGenerator.Create(),
                order.Id,
                product.Product.Id.ToString(),
                product.Product.Price,
                product.Amount,
                product.Amount *  product.Product.Price,
                product.Product.Name
            );
                
            await _orderLineRepository.InsertAsync(orderLine);
            order.OrderLines.Add(orderLine);
                
            // TODO: Discard this event and handle OrderCreatedEto in catalog module
            await _eventBus.PublishAsync(new ProductStockDecreaseEto
            {
                ProductId = product.Product.Id,
                Amount = product.Amount,
            });
        }
            
        await _eventBus.PublishAsync(new OrderCreatedEto
        {
            Id = order.Id,
            UserId = _currentUser.GetId(),
            Products = productDtos
        });

        // TODO: Discard this event and use OrderCreatedEto in payment module
        await _eventBus.PublishAsync(new PaymentOrderCreatedEto
        {
            OrderId = order.Id,
            TotalPrice = order.TotalPrice
        });
            
        return order;
    }

    public virtual async Task CancelAsync(Guid id)
    {
        var order = await _orderRepository.GetAsync(id);

        if (order.State is OrderState.Shipped or OrderState.Completed or OrderState.Cancelled)
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

    public virtual async Task<Order> UpdateShippingAddressAsync(
        Guid id,
        string shippingAddress,
        string? concurrencyStamp = null)
    {
        Check.NotNullOrWhiteSpace(shippingAddress, nameof(shippingAddress));
        Check.Length(shippingAddress, nameof(shippingAddress), OrderConsts.ShippingAddressMaxLength);

        var order = await _orderRepository.GetAsync(id);

        order.ShippingAddress = shippingAddress;

        order.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _orderRepository.UpdateAsync(order);
    }

    public virtual async Task<Order> UpdateStateAsync(
        Guid id,
        OrderState state)
    {
        Check.NotNull(state, nameof(state));

        var order = await _orderRepository.GetAsync(id);

        order.State = state;

        return await _orderRepository.UpdateAsync(order);
    }
}