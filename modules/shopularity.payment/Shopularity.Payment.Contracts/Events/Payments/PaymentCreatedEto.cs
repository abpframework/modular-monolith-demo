using System;

namespace Shopularity.Payment.Events.Payments;

public class PaymentCreatedEto
{
    public Guid PaymentId { get; set; }

    public Guid OrderId { get; set; }
}