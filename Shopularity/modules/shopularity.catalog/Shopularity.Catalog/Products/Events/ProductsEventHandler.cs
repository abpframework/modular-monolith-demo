using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopularity.Catalog.Products.Admin;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.ObjectMapping;

namespace Shopularity.Catalog.Products.Events;

public class ProductsEventHandler : IDistributedEventHandler<ProductsRequestedEto>
{
    private readonly IDistributedEventBus _eventBus;
    private readonly IProductRepository _productRepository;
    private readonly IObjectMapper _objectMapper;

    public ProductsEventHandler(IDistributedEventBus eventBus, IProductRepository productRepository, IObjectMapper objectMapper)
    {
        _eventBus = eventBus;
        _productRepository = productRepository;
        _objectMapper = objectMapper;
    }
    
    public async Task HandleEventAsync(ProductsRequestedEto eventData)
    {
        var products = await _productRepository.GetListAsync(eventData.Products.Select(x=> x.Key).ToList());
        var productsAsDto = _objectMapper.Map<List<Product>, List<ProductDto>>(products);

        await _eventBus.PublishAsync(new ProductsRequestCompletedEto
        {
            Products = productsAsDto.Select(x=>
                 new KeyValuePair<ProductDto, int>(x, eventData.Products[x.Id])
                ).ToDictionary(),
            RequesterId = eventData.RequesterId
        });
    }
}