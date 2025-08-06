using Shopularity.Basket.Tests;
using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("Shopularity.Basket.csproj"); 
await builder.RunAbpModuleAsync<BasketTestsModule>(applicationName: "Shopularity.Basket");

public partial class TestProgram
{
}
