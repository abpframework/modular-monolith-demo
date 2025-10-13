using Volo.Abp.Identity;
using Shopularity.Payment;
using Shopularity.Catalog;
using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using Shopularity.Ordering.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Shopularity.Ordering;

[DependsOn(
    typeof(AbpIdentityApplicationContractsModule),
    typeof(PaymentContractsModule),
    typeof(CatalogContractsModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpValidationModule),
    typeof(AbpAuthorizationModule)
)]
public class OrderingContractsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<OrderingContractsModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<OrderingResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/Ordering");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Ordering", typeof(OrderingResource));
        });
    }
}
