using Shopularity.Catalog.Categories;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Shopularity.Catalog.Data;

[ConnectionStringName(CatalogDbProperties.ConnectionStringName)]
public interface ICatalogDbContext : IEfCoreDbContext
{
    DbSet<Category> Categories { get; set; }
}