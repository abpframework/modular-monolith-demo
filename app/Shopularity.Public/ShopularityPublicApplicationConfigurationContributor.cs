using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Shopularity.Public;

public class ShopularityPublicApplicationConfigurationContributor : IApplicationConfigurationContributor
{
    private IConfiguration Configuration { get; }

    public ShopularityPublicApplicationConfigurationContributor(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public Task ContributeAsync(ApplicationConfigurationContributorContext context)
    {
        context.ApplicationConfiguration.SetProperty("remoteServiceUrl", Configuration["RemoteServices:Default:BaseUrl"]);

        return Task.CompletedTask;
    }
}