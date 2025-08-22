namespace Shopularity.Ordering.Orders;

public static class OrderConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Order." : string.Empty);
    }

    public const int ShippingAddressMinLength = 0;
    public const int ShippingAddressMaxLength = 256;
    public const int CargoNoMinLength = 0;
    public const int CargoNoMaxLength = 32;
}