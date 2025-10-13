using Shopularity.Ordering.Tests;
using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("Shopularity.Ordering.csproj"); 
await builder.RunAbpModuleAsync<OrderingTestsModule>(applicationName: "Shopularity.Ordering");

public partial class TestProgram
{
}
