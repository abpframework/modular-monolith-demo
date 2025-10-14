using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Shopularity.Public;

[Dependency(ReplaceServices = true)]
public class BrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Shopularity";
}