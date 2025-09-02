using Volo.Abp.BackgroundJobs;
using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using Shopularity.Payment.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Shopularity.Payment;

[DependsOn(
    typeof(AbpBackgroundJobsAbstractionsModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpValidationModule),
    typeof(AbpAuthorizationModule)
)]
public class PaymentContractsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<PaymentContractsModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<PaymentResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/Payment");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Payment", typeof(PaymentResource));
        });
    }
}
