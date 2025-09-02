using Shopularity.Ordering;
using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using Shopularity.Basket.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Shopularity.Basket;

[DependsOn(
    typeof(OrderingContractsModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpValidationModule),
    typeof(AbpAuthorizationModule)
)]
public class BasketContractsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<BasketContractsModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<BasketResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/Basket");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Basket", typeof(BasketResource));
        });
    }
}
