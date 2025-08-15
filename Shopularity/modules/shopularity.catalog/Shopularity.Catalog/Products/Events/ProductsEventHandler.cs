using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopularity.Catalog.Products.Admin;
using Volo.Abp;
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

        foreach (var product in products)
        {
            var amount = eventData.Products[product.Id];

            if (amount > product.StockCount)
            {
                //todo: business exception
                throw new UserFriendlyException("Not Enough Stock!!");
            }

            product.StockCount -= amount;

            await _productRepository.UpdateAsync(product);
        }
        
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