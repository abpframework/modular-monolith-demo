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

        return Task.CompletedTask;
    }
}
