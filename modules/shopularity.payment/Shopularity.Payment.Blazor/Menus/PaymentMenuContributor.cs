using Shopularity.Payment.Permissions;
using Shopularity.Payment.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace Shopularity.Payment.Blazor.Menus;

public class PaymentMenuContributor : IMenuContributor
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
        AddMenuItemPayments(context, moduleMenu);
        
        return Task.CompletedTask;
    }

    private static ApplicationMenuItem AddModuleMenuItem(MenuConfigurationContext context)
    {
        var moduleMenu = new ApplicationMenuItem(
            PaymentMenus.Prefix,
            context.GetLocalizer<PaymentResource>()["Menu:Payment"],
            icon: "fa fa-folder"
        );

        context.Menu.Items.AddIfNotContains(moduleMenu);
        return moduleMenu;
    }
    private static void AddMenuItemPayments(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                PaymentMenus.Payments,
                context.GetLocalizer<PaymentResource>()["Menu:Payments"],
                "/Payment/Payments",
                icon: "fa fa-file-alt",
                requiredPermissionName: PaymentPermissions.Payments.Default
            )
        );
    }
}