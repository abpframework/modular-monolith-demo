using Shopularity.Ordering.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Shopularity.Ordering.Permissions;

public class OrderingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(OrderingPermissions.GroupName, L("Permission:Ordering"));

        var orderPermission = myGroup.AddPermission(OrderingPermissions.Orders.Default, L("Permission:Orders"));
        orderPermission.AddChild(OrderingPermissions.Orders.Create, L("Permission:Create"));
        orderPermission.AddChild(OrderingPermissions.Orders.Edit, L("Permission:Edit"));
        orderPermission.AddChild(OrderingPermissions.Orders.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<OrderingResource>(name);
    }
}