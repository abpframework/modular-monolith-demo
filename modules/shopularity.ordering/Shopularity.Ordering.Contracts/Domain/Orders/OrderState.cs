namespace Shopularity.Ordering.Domain.Orders;

public enum OrderState
{
    New,
    WaitingForPayment,
    Paid,
    Processing,
    Shipped,
    Completed,
    Cancelled
}