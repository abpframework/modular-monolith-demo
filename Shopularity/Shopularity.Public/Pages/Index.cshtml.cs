using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopularity.Catalog.Products;
using Shopularity.Catalog.Products.Public;

namespace Shopularity.Public.Pages;

public class IndexModel : ShopularityPublicPageModel
{
    private readonly IProductsPublicAppService _productsAppService;
    
    public List<ProductWithNavigationPropertiesPublicDto> MyProducts = new ();

    public IndexModel(IProductsPublicAppService productsAppService)
    {
        _productsAppService = productsAppService;
    }
    
    public async Task OnGetAsync()
    {
        MyProducts = (await _productsAppService.GetListAsync(new GetProductsInput())).Items.ToList();
    }
}