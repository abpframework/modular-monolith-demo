namespace Shopularity.Payment.Domain.Payments;

public static class PaymentConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Payment." : string.Empty);
    }
}