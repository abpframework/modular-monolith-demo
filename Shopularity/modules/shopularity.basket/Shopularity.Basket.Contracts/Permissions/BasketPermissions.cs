using Volo.Abp.Reflection;

namespace Shopularity.Basket.Permissions;

public class BasketPermissions
{
    public const string GroupName = "Basket";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(BasketPermissions));
    }
}
