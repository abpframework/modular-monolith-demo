using System;

namespace Shopularity.Payment.Payments.Events;

public class PaymentOrderCreatedEto
{
    public Guid OrderId { get; set; }
    
    public double TotalPrice { get; set; }
}