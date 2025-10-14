namespace Shopularity.Ordering.Domain.Orders;

public static class OrderStateExtensions
{
    public static bool IsShipped(this OrderState state)
    {
        return  state is OrderState.Shipped or OrderState.Completed;
    }
}