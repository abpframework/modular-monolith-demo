using System;

namespace Shopularity.Payment.Payments.Events;

public class PaymentCompletedEto
{
    public Guid PaymentId { get; set; }
    
    public string OrderId { get; set; }
}