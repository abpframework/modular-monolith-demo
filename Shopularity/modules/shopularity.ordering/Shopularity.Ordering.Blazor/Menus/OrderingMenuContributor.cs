using Shopularity.Ordering.Permissions;
using Shopularity.Ordering.Localization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace Shopularity.Ordering.Blazor.Menus;

public class OrderingMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }

        var moduleMenu = AddModuleMenuItem(context);
        AddMenuItemOrders(context, moduleMenu);
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {

        return Task.CompletedTask;
    }

    private static ApplicationMenuItem AddModuleMenuItem(MenuConfigurationContext context)
    {
        var moduleMenu = new ApplicationMenuItem(
            OrderingMenus.Prefix,
            context.GetLocalizer<OrderingResource>()["Menu:Ordering"],
            icon: "fa fa-folder"
        );

        context.Menu.Items.AddIfNotContains(moduleMenu);
        return moduleMenu;
    }
    private static void AddMenuItemOrders(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.OrderingMenus.Orders,
                context.GetLocalizer<OrderingResource>()["Menu:Orders"],
                "/Ordering/Orders",
                icon: "fa fa-file-alt",
                requiredPermissionName: OrderingPermissions.Orders.Default
            )
        );
    }
}