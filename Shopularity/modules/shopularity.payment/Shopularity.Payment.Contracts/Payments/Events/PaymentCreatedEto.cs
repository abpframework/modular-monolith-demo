using System;

namespace Shopularity.Payment.Payments.Events;

public class PaymentCreatedEto
{
    public Guid PaymentId { get; set; }

    public Guid OrderId { get; set; }
}