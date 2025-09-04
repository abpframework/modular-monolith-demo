using Microsoft.EntityFrameworkCore;
using Shopularity.Catalog.Domain.Categories;
using Shopularity.Catalog.Domain.Products;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Shopularity.Catalog.Data;

[ConnectionStringName(CatalogDbProperties.ConnectionStringName)]
public interface ICatalogDbContext : IEfCoreDbContext
{
    DbSet<Product> Products { get; set; }
    DbSet<Category> Categories { get; set; }
}