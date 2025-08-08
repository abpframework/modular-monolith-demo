using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopularity.Catalog.Products;
using Shopularity.Catalog.Products.Public;

namespace Shopularity.Public.Pages;

public class IndexModel : ShopularityPublicPageModel
{
    public IProductsPublicAppService ProductsAppService { get; set; }
    
    public List<ProductWithNavigationPropertiesPublicDto> MyProducts  { get; set; }

    public async Task OnGetAsync()
    {
        MyProducts = (await ProductsAppService.GetListAsync(new GetProductsInput())).Items.ToList();
    }
}