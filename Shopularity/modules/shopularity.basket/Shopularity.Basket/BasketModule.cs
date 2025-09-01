using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.EntityFrameworkCore;
using Shopularity.Basket.Data;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.SignalR;

namespace Shopularity.Basket;

[DependsOn(
    typeof(BasketContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAspNetCoreSignalRModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class BasketModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(BasketModule).Assembly);
        });
    }
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<BasketModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<BasketModule>(validate: true);
        });
        
        context.Services.AddAbpDbContext<BasketDbContext>(options =>
        {
            options.AddDefaultRepositories<IBasketDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
        });
    }
}
