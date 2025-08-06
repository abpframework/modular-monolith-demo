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

        return Task.CompletedTask;
    }
}
