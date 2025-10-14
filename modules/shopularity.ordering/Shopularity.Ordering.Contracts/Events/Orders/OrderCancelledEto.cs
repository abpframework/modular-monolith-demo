using System;

namespace Shopularity.Ordering.Events.Orders;

public class OrderCancelledEto
{
    public Guid OrderId { get; set; }
}