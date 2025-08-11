using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Public.Components.Toolbar.BasketToolbarItem;

public class BasketToolbarItemViewComponent : AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("/Components/Toolbar/BasketToolbarItem/Default.cshtml");
    }
}
