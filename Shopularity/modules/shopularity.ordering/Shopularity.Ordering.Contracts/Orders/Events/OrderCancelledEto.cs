using System;

namespace Shopularity.Ordering.Orders.Events;

public class OrderCancelledEto
{
    public Guid OrderId { get; set; }
}