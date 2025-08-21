namespace Shopularity.Payment.Payments;

public enum PaymentState
{
    Waiting,
    Completed,
    Cancelled,
    Refunded,
    Failed
}