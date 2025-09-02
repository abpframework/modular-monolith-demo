using System.Threading.Tasks;
using Shopularity.Ordering.Orders.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Shopularity.Catalog.Products.Events;

public class ProductsEventHandler : 
    IDistributedEventHandler<OrderCreatedEto>,
    ITransientDependency
{
    private readonly ProductManager _productManager;

    public ProductsEventHandler(ProductManager productManager)
    {
        _productManager = productManager;
    }
    
    public async Task HandleEventAsync(OrderCreatedEto eventData)
    {
        foreach (var product in eventData.ProductsWithAmounts)
        {
            await _productManager.DecreaseCountAsync(product.Key, product.Value);
        }
    }
}