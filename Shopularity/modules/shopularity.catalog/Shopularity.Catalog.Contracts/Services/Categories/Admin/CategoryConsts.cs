namespace Shopularity.Catalog.Services.Categories.Admin;

public static class CategoryConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Category." : string.Empty);
    }

    public const int NameMinLength = 0;
    public const int NameMaxLength = 256;
}