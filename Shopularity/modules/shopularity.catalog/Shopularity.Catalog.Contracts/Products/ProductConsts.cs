namespace Shopularity.Catalog.Products
{
    public static class ProductConsts
    {
        private const string DefaultSorting = "{0}CreationTime desc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Product." : string.Empty);
        }

        public const int NameMinLength = 0;
        public const int NameMaxLength = 512;
        public const int DescriptionMinLength = 0;
        public const int DescriptionMaxLength = 2048;
    }
}