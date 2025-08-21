using System;

namespace Shopularity.Payment.Payments.Events;

public class PaymentCreatedEto
{
    public Guid OrderId { get; set; }
}