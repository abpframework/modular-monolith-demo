namespace Shopularity.Ordering.Domain.Orders;

public static class OrderStateExtensions
{
    public static bool IsPostShippingState(this OrderState state)
    {
        return  state is OrderState.Shipped or OrderState.Completed;
    }
}