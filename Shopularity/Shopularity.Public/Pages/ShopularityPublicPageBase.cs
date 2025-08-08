using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.Extensions.Localization;
using Shopularity.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Layout;

namespace Shopularity.Public.Pages;

public class ShopularityPublicPageBase : Microsoft.AspNetCore.Mvc.RazorPages.Page
{
    [RazorInject] public IStringLocalizer<ShopularityResource> L { get; set; }

    [RazorInject] public IPageLayout PageLayout { get; set; }

    public override Task ExecuteAsync()
    {
        return Task.CompletedTask;
    }
}
