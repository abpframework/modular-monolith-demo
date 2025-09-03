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

//TODO: Discard this class
public class OrderManager : DomainService
{
    protected IOrderRepository _orderRepository;
    private readonly IOrderLineRepository _orderLineRepository; //TODO: Remove unused ones
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

    public virtual async Task<Order> CreateNewAsync( //TODO: Discard this method
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
                
            order.AddOrderLine(orderLine);
        }
        
        return order;
    }

    public virtual async Task<Order> UpdateShippingAddressAsync( //TODO: Discard this method
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

    public virtual async Task<Order> UpdateStateAsync( //TODO: Discard this method
        Guid id,
        OrderState state)
    {
        Check.NotNull(state, nameof(state));

        var order = await _orderRepository.GetAsync(id);

        order.State = state;

        return await _orderRepository.UpdateAsync(order);
    }
}