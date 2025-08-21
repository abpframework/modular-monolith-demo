using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Shopularity.Catalog.Products.Events;

public class ProductsEventHandler : IDistributedEventHandler<ProductStockDecreaseEto>,
    ITransientDependency
{
    private readonly ProductManager _productManager;

    public ProductsEventHandler(ProductManager productManager)
    {
        _productManager = productManager;
    }
    
    public async Task HandleEventAsync(ProductStockDecreaseEto eventData)
    {
        await _productManager.DecreaseCountAsync(eventData.ProductId, eventData.Amount);
    }
}