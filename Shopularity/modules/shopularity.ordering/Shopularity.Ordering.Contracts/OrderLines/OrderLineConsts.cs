namespace Shopularity.Ordering.OrderLines
{
    public static class OrderLineConsts
    {
        private const string DefaultSorting = "{0}CreationTime desc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "OrderLine." : string.Empty);
        }

        public const int NameMaxLength = 256;
    }
}