using Shopularity.Catalog.Permissions;
using Shopularity.Catalog.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace Shopularity.Catalog.Blazor.Menus;

public class CatalogMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var moduleMenu = AddModuleMenuItem(context);
        AddMenuItemCategories(context, moduleMenu);
        AddMenuItemProducts(context, moduleMenu);

        return Task.CompletedTask;
    }

    private static ApplicationMenuItem AddModuleMenuItem(MenuConfigurationContext context)
    {
        var moduleMenu = new ApplicationMenuItem(
            CatalogMenus.Prefix,
            context.GetLocalizer<CatalogResource>()["Menu:Catalog"],
            icon: "fa fa-folder"
        );

        context.Menu.Items.AddIfNotContains(moduleMenu);
        return moduleMenu;
    }

    private static void AddMenuItemCategories(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.CatalogMenus.Categories,
                context.GetLocalizer<CatalogResource>()["Menu:Categories"],
                "/Catalog/Categories",
                icon: "fa fa-file-alt",
                requiredPermissionName: CatalogPermissions.Categories.Default
            )
        );
    }

    private static void AddMenuItemProducts(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.CatalogMenus.Products,
                context.GetLocalizer<CatalogResource>()["Menu:Products"],
                "/Catalog/Products",
                icon: "fa fa-file-alt",
                requiredPermissionName: CatalogPermissions.Products.Default
            )
        );
    }
}