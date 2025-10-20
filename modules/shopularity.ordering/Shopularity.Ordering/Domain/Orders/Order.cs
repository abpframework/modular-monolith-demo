using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Shopularity.Ordering.Domain.Orders.OrderLines;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Shopularity.Ordering.Domain.Orders;

public class Order : FullAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual Guid UserId { get; set; }

    public virtual OrderState State { get; private set; }

    public virtual double TotalPrice { get; set; }

    [NotNull]
    public virtual string ShippingAddress { get; set; }

    [CanBeNull]
    public virtual string? CargoNo { get; set; }

    public ICollection<OrderLine> OrderLines { get; private set; }

    protected Order()
    {

    }

    public Order(Guid id, Guid userId, double totalPrice, string shippingAddress)
    {
        Id = id;
        Check.NotNull(userId, nameof(userId));
        Check.NotNull(shippingAddress, nameof(shippingAddress));
        Check.Length(shippingAddress, nameof(shippingAddress), OrderConsts.ShippingAddressMaxLength, OrderConsts.ShippingAddressMinLength);
        UserId = userId;
        State = OrderState.New;
        TotalPrice = totalPrice;
        ShippingAddress = shippingAddress;
        OrderLines = new Collection<OrderLine>();
    }

    public void AddOrderLine(OrderLine orderLine)
    {
        OrderLines.Add(orderLine);
    }

    public void SetState(OrderState newState)
    {
        if (newState == OrderState.Cancelled && State.IsPostShippingState())
        {
            throw new BusinessException(OrderingErrorCodes.CanOnlyCancelNotShippedOrders);
        }
        
        State = OrderState.Cancelled;
    }
}