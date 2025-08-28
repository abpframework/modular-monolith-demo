namespace Shopularity.Ordering;

public static class OrderingErrorCodes
{
    public const string CanOnlyCancelOwnedOrders = "Ordering:CanOnlyCancelOwnedOrders";
    public const string CanOnlyCancelNotShippedOrders = "Ordering:CanOnlyCancelNotShippedOrders";
    public const string OrderIsNotAvailableYetForShipping = "Ordering:OrderIsNotAvailableYetForShipping";
    
}
