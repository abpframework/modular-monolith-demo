using Volo.Abp.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Shopularity.Data;

public class ShopularityDbSchemaMigrator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public ShopularityDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        
        /* We intentionally resolving the ShopularityDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<ShopularityDbContext>()
            .Database
            .MigrateAsync();

    }
}
