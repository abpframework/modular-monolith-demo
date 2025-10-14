using Shopularity.Catalog.Tests;
using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("Shopularity.Catalog.csproj"); 
await builder.RunAbpModuleAsync<CatalogTestsModule>(applicationName: "Shopularity.Catalog");

public partial class TestProgram
{
}
