namespace Shopularity.Payment.Data;

public static class PaymentDbProperties
{
    public static string DbTablePrefix { get; set; } = "Payment";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Payment";
}
