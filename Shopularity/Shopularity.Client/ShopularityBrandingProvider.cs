using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using Shopularity.Localization;

namespace Shopularity;

[Dependency(ReplaceServices = true)]
public class ShopularityBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<ShopularityResource> _localizer;

    public ShopularityBrandingProvider(IStringLocalizer<ShopularityResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
    public override string? LogoUrl => "/images/logo/leptonxlite/logo-light.png";
}
