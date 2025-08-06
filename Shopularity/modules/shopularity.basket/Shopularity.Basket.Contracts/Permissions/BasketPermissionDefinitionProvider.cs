using Shopularity.Basket.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Shopularity.Basket.Permissions;

public class BasketPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(BasketPermissions.GroupName, L("Permission:Basket"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BasketResource>(name);
    }
}
