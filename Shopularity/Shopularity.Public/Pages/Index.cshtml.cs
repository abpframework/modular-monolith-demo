using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopularity.Catalog.Products;

namespace Shopularity.Public.Pages;

public class IndexModel : ShopularityPublicPageModel
{
    private readonly IProductsAppService _productsAppService;
    
    public List<ProductWithNavigationPropertiesDto> MyProducts = new ();

    public IndexModel(IProductsAppService productsAppService)
    {
        _productsAppService = productsAppService;
    }
    
    public async Task OnGetAsync()
    {
        MyProducts = (await _productsAppService.GetListAsync(new GetProductsInput())).Items.ToList();
    }
}