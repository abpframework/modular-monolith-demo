using System;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Volo.Abp.AspNetCore.Authentication.OpenIdConnect;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Shopularity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.AspNetCore.Serilog;
using Microsoft.OpenApi.Models;
using Shopularity.Basket;
using Shopularity.Catalog;
using Shopularity.Ordering;
using Shopularity.Payment;
using Shopularity.Public.Menus;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.Http.Client;
using Volo.Abp.Security.Claims;
using Volo.Abp.Studio.Client.AspNetCore;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation;

namespace Shopularity.Public;

[DependsOn(
    typeof(AbpAspNetCoreAuthenticationOpenIdConnectModule),
    typeof(AbpHttpClientIdentityModelWebModule),
    typeof(ShopularityContractsModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpHttpClientModule),
    typeof(AbpStudioClientAspNetCoreModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule)
    )]
public class ShopularityPublicWebHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        ConfigureSwagger(context, configuration);
        ConfigureAuthentication(context, configuration);
        ConfigureNavigationServices(configuration);
        ConfigureHttpClientProxies(context);
    }

    private static void ConfigureHttpClientProxies(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(typeof(ShopularityContractsModule).Assembly);
        context.Services.AddHttpClientProxies(typeof(CatalogContractsModule).Assembly);
        context.Services.AddHttpClientProxies(typeof(OrderingContractsModule).Assembly);
        context.Services.AddHttpClientProxies(typeof(PaymentContractsModule).Assembly);
        context.Services.AddHttpClientProxies(typeof(BasketContractsModule).Assembly);
    }

    private void ConfigureNavigationServices(IConfiguration configuration)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new ShopularityPublicMenuContributor(configuration));
        });

        Configure<AbpToolbarOptions>(options =>
        {
            options.Contributors.Add(new ShopularityToolbarContributor());
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = "Cookies";
            options.DefaultChallengeScheme = "oidc";
        })
        .AddCookie("Cookies", options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromDays(365);
            options.CheckTokenExpiration();
        })
        .AddAbpOpenIdConnect("oidc", options =>
        {
            options.Authority = configuration["AuthServer:Authority"];
            options.RequireHttpsMetadata = configuration.GetValue<bool>("AuthServer:RequireHttpsMetadata"); ;
            options.ResponseType = OpenIdConnectResponseType.CodeIdToken;

            options.ClientId = configuration["AuthServer:ClientId"];
            options.ClientSecret = configuration["AuthServer:ClientSecret"];

            options.UsePkce = true;
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;

            options.Scope.Add("roles");
            options.Scope.Add("email");
            options.Scope.Add("phone");
            options.Scope.Add("Shopularity");
        });
        
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
    }

    private void ConfigureSwagger(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAbpSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo {Title = "Shopularity.Public API", Version = "v1"});
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();
        app.MapAbpStaticAssets();
        app.UseRouting();
        app.UseAuthorization();
        app.UseAbpStudioLink();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
           options.SwaggerEndpoint("/swagger/v1/swagger.json", "Shopularity.Public API");
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}