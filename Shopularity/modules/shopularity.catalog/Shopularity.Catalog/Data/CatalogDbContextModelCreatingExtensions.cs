using Volo.Abp.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore;
using Shopularity.Catalog.Categories;
using Shopularity.Catalog.Products;
using Shopularity.Catalog.Products.Admin;
using Volo.Abp;

namespace Shopularity.Catalog.Data;

public static class CatalogDbContextModelCreatingExtensions
{
    public static void ConfigureCatalog(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        if (builder.IsHostDatabase())
        {
            builder.Entity<Category>(b =>
            {
                b.ToTable(CatalogDbProperties.DbTablePrefix + "Categories", CatalogDbProperties.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).HasColumnName(nameof(Category.Name)).IsRequired().HasMaxLength(CategoryConsts.NameMaxLength);
            });
        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Product>(b =>
            {
                b.ToTable(CatalogDbProperties.DbTablePrefix + "Products", CatalogDbProperties.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).HasColumnName(nameof(Product.Name)).IsRequired().HasMaxLength(ProductConsts.NameMaxLength);
                b.Property(x => x.Description).HasColumnName(nameof(Product.Description)).HasMaxLength(ProductConsts.DescriptionMaxLength);
                b.Property(x => x.Price).HasColumnName(nameof(Product.Price));
                b.Property(x => x.StockCount).HasColumnName(nameof(Product.StockCount));
                b.HasOne<Category>().WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.SetNull);
            });

        }
    }
}