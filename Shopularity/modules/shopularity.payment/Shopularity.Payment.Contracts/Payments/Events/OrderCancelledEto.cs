using System;

namespace Shopularity.Payment.Payments.Events;

public class OrderCancelledEto
{
    public Guid OrderId { get; set; }
}