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
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
      
        return Task.CompletedTask;
    }
}
