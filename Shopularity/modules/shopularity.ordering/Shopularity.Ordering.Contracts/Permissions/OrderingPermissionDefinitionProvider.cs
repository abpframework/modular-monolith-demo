using Shopularity.Ordering.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Shopularity.Ordering.Permissions;

public class OrderingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var orderingGroup = context.AddGroup(OrderingPermissions.GroupName, L("Permission:Ordering"));

        var orderPermission = orderingGroup.AddPermission(OrderingPermissions.Orders.Default, L("Permission:Orders"));
        orderPermission.AddChild(OrderingPermissions.Orders.Edit, L("Permission:Edit"));
        orderPermission.AddChild(OrderingPermissions.Orders.SetShippingInfo, L("Permission:SetShippingInfo"));

        orderingGroup.AddPermission(OrderingPermissions.OrderLines.Default, L("Permission:OrderLines"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<OrderingResource>(name);
    }
}