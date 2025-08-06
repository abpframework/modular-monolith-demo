using Shopularity.Payment.Tests;
using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("Shopularity.Payment.csproj"); 
await builder.RunAbpModuleAsync<PaymentTestsModule>(applicationName: "Shopularity.Payment");

public partial class TestProgram
{
}
