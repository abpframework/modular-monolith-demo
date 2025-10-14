using Shopularity.Payment.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Shopularity.Payment.Permissions;

public class PaymentPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var permissionGroup = context.AddGroup(PaymentPermissions.GroupName, L("Permission:Payment"));

        permissionGroup.AddPermission(PaymentPermissions.Payments.Default, L("Permission:Payments"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<PaymentResource>(name);
    }
}