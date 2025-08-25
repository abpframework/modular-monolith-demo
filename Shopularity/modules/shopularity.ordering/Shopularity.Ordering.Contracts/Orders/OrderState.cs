namespace Shopularity.Ordering.Orders;

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

public static class OrderStateExtensions
{
    public static bool IsShipped(this OrderState state)
    {
        return  state is OrderState.Shipped or OrderState.Completed;
    }
}