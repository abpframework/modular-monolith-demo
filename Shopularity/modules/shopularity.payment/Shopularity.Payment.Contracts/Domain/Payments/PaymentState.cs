namespace Shopularity.Payment.Domain.Payments;

public enum PaymentState
{
    Waiting,
    Completed,
    Failed,
    Cancelled,
    Refunded
}