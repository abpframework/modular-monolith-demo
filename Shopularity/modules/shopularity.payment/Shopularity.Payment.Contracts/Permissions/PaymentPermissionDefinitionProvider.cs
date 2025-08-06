using Shopularity.Payment.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Shopularity.Payment.Permissions;

public class PaymentPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(PaymentPermissions.GroupName, L("Permission:Payment"));

        var paymentPermission = myGroup.AddPermission(PaymentPermissions.Payments.Default, L("Permission:Payments"));
        paymentPermission.AddChild(PaymentPermissions.Payments.Create, L("Permission:Create"));
        paymentPermission.AddChild(PaymentPermissions.Payments.Edit, L("Permission:Edit"));
        paymentPermission.AddChild(PaymentPermissions.Payments.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<PaymentResource>(name);
    }
}