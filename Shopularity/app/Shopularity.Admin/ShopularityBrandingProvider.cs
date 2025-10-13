using Microsoft.Extensions.Localization;
using Shopularity.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

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
