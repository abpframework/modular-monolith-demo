using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.EntityFrameworkCore;
using Shopularity.Payment.Data;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Payment;

[DependsOn(
    typeof(PaymentContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class PaymentModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(PaymentModule).Assembly);
        });
    }
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<PaymentModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<PaymentModule>(validate: true);
        });
        
        context.Services.AddAbpDbContext<PaymentDbContext>(options =>
        {
            options.AddDefaultRepositories<IPaymentDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
        });
    }
}
