using Volo.Abp.Reflection;

namespace Shopularity.Ordering.Permissions;

public class OrderingPermissions
{
    public const string GroupName = "Ordering";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(OrderingPermissions));
    }

    public static class Orders
    {
        public const string Default = GroupName + ".Orders";
        public const string SetShippingInfo = Default + ".SetShippingInfo";
        public const string Edit = Default + ".Edit";
    }

    public static class OrderLines
    {
        public const string Default = GroupName + ".OrderLines";
    }
}