using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Shopularity.Catalog.Products;
using Shopularity.Catalog.Categories;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.EntityFrameworkCore;
using Shopularity.Catalog.Data;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Catalog;

[DependsOn(
    typeof(BlobStoringDatabaseEntityFrameworkCoreModule),
    typeof(CatalogContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class CatalogModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(CatalogModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<CatalogModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<CatalogModule>(validate: true);
        });

        context.Services.AddAbpDbContext<CatalogDbContext>(options =>
        {
            options.AddDefaultRepositories<ICatalogDbContext>(includeAllEntities: true);

            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
            options.AddRepository<Category, Categories.EfCoreCategoryRepository>();

            options.AddRepository<Product, Products.EfCoreProductRepository>();

        });
    }
}