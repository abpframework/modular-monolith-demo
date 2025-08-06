namespace Shopularity.Basket;

public static class BasketDbProperties
{
    public static string DbTablePrefix { get; set; } = "Basket";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Basket";
}
