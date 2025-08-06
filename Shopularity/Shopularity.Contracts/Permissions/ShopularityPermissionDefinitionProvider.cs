using Shopularity.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Shopularity.Permissions;

public class ShopularityPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ShopularityPermissions.GroupName);



        //Define your own permissions here. Example:
        //myGroup.AddPermission(ShopularityPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ShopularityResource>(name);
    }
}
