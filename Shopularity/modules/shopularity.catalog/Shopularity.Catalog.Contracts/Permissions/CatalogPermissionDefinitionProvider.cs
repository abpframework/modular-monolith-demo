using Shopularity.Catalog.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Shopularity.Catalog.Permissions;

public class CatalogPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CatalogPermissions.GroupName, L("Permission:Catalog"));

        var categoryPermission = myGroup.AddPermission(CatalogPermissions.Categories.Default, L("Permission:Categories"));
        categoryPermission.AddChild(CatalogPermissions.Categories.Create, L("Permission:Create"));
        categoryPermission.AddChild(CatalogPermissions.Categories.Edit, L("Permission:Edit"));
        categoryPermission.AddChild(CatalogPermissions.Categories.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CatalogResource>(name);
    }
}