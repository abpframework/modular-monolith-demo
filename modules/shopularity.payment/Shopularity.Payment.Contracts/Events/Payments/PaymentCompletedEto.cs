using System;

namespace Shopularity.Payment.Events.Payments;

public class PaymentCompletedEto
{
    public Guid PaymentId { get; set; }
    
    public Guid OrderId { get; set; }
}