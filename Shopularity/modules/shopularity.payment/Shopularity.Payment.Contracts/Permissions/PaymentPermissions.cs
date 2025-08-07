using Volo.Abp.Reflection;

namespace Shopularity.Payment.Permissions;

public class PaymentPermissions
{
    public const string GroupName = "Payment";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(PaymentPermissions));
    }

    public static class Payments
    {
        public const string Default = GroupName + ".Payments";
    }
}