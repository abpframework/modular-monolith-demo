using Volo.CmsKit.EntityFrameworkCore;
using Shopularity.Basket.Data;
using Shopularity.Catalog.Data;
using Shopularity.Payment.Data;
using Shopularity.Ordering.Data;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Shopularity.Data;

public class ShopularityDbContext : AbpDbContext<ShopularityDbContext>
{
    public const string DbTablePrefix = "App";
    public const string DbSchema = null;

    public ShopularityDbContext(DbContextOptions<ShopularityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigurePermissionManagement();
        builder.ConfigureBlobStoring();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureTenantManagement();
        builder.ConfigureBasket();
        builder.ConfigureCatalog();
        builder.ConfigurePayment();
        builder.ConfigureOrdering();
        builder.ConfigureCmsKit();
    }
}

