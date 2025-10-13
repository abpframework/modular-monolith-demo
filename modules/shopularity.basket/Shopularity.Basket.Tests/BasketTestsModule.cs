using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Caching;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Basket.Tests;

[DependsOn(
    typeof(AbpAspNetCoreTestBaseModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpEventBusModule),
    typeof(AbpCachingModule),
    typeof(AbpDistributedLockingAbstractionsModule)
)]
[AdditionalAssembly(typeof(BasketModule))]
public class BasketTestsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        ConfigureAuthorization(context);
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();

        app.UseCorrelationId();
        app.UseAbpRequestLocalization();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseUnitOfWork();
        app.UseConfiguredEndpoints();
    }

    private static void ConfigureAuthorization(ServiceConfigurationContext context)
    {
        /* We don't need to authorization in tests */
        context.Services.AddAlwaysAllowAuthorization();
    }
}
