namespace Shopularity.Payment.Payments;

public enum PaymentState
{
    Waiting,
    Completed,
    Failed,
    Cancelled,
    Refunded
}