using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace Shopularity.Basket.Blazor.Menus;

public class BasketMenuContributor : IMenuContributor
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
        //Add main menu items.
        context.Menu.AddItem(new ApplicationMenuItem(BasketMenus.Prefix, displayName: "Basket", "/Basket", icon: "fa fa-globe"));

        return Task.CompletedTask;
    }
}
