using Volo.Abp.Reflection;

namespace Shopularity.Ordering.Permissions;

public class OrderingPermissions
{
    public const string GroupName = "Ordering";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(OrderingPermissions));
    }
}
