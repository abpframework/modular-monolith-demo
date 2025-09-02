namespace Shopularity.Ordering;

public static class OrderingErrorCodes
{
    public const string CanOnlyCancelOwnedOrders = "Ordering:CanOnlyCancelOwnedOrders";
    public const string CanOnlyCancelNotShippedOrders = "Ordering:CanOnlyCancelNotShippedOrders";
    public const string OrderIsNotAvailableYetForShipping = "Ordering:OrderIsNotAvailableYetForShipping";
    public const string OrderShouldContainProducts = "Ordering:OrderShouldContainProducts";
    public const string NotEnoughStock = "Ordering:NotEnoughStock";
}
