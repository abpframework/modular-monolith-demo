using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Shopularity.Data;

public class ShopularityDbContextFactory : IDesignTimeDbContextFactory<ShopularityDbContext>
{
    public ShopularityDbContext CreateDbContext(string[] args)
    {
        ShopularityGlobalFeatureConfigurator.Configure();
        
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<ShopularityDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new ShopularityDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}