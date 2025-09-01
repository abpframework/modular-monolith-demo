using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Catalog.Categories.Public;

namespace Shopularity.Public.Pages;

public class IndexModel : ShopularityPublicPageModel
{
    public ICategoriesPublicAppService CategoriesAppService { get; }

    public List<CategoryPublicDto> Categories { get; set; }

    public IndexModel(ICategoriesPublicAppService categoriesAppService)
    {
        CategoriesAppService = categoriesAppService;
    }
    
    public virtual async Task<ActionResult> OnGetAsync()
    {
        var result = await CategoriesAppService.GetListAsync();
        Categories = result.Items.ToList();

        return Page();
    }
}