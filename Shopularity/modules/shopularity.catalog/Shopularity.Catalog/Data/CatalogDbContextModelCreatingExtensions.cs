using Volo.Abp.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore;
using Shopularity.Catalog.Categories;
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
    }
}