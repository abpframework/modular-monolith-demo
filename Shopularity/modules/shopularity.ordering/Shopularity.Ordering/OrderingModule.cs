using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.EntityFrameworkCore;
using Shopularity.Ordering.Data;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Ordering;

[DependsOn(
    typeof(OrderingContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class OrderingModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(OrderingModule).Assembly);
        });
    }
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<OrderingModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<OrderingModule>(validate: true);
        });
        
        context.Services.AddAbpDbContext<OrderingDbContext>(options =>
        {
            options.AddDefaultRepositories<IOrderingDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
        });
    }
}
