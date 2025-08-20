using Shopularity.Basket.Domain;
using Shopularity.Basket.Services;
using Shopularity.Catalog.Products;
using Shopularity.Catalog.Products.Events;
using Shopularity.Catalog.Products.Public;
using Shopularity.Ordering.OrderLines;
using Shopularity.Ordering.Orders.Events;
using Shopularity.Payment.Payments;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace Shopularity.Events;

public class ShopularityEventHandler:
    IDistributedEventHandler<OrderCreatedEto>,
    IDistributedEventHandler<ProductsRequestCompletedEto>,
    IDistributedEventHandler<OrderLinesProcessedDto>,
    ITransientDependency
{
    private readonly OrderLineManager _orderLineManager;
    private readonly IDistributedEventBus _eventBus;
    private readonly BasketManager _basketManager;
    private readonly ProductManager _productManager;
    private readonly PaymentManager _paymentManager;
    private readonly IProductsPublicAppService _productsPublicAppService;

    public ShopularityEventHandler(
        OrderLineManager orderLineManager,
        IDistributedEventBus eventBus,
        BasketManager basketManager,
        ProductManager productManager,
        PaymentManager paymentManager,
        IProductsPublicAppService productsPublicAppService)
    {
        _orderLineManager = orderLineManager;
        _eventBus = eventBus;
        _basketManager = basketManager;
        _productManager = productManager;
        _paymentManager = paymentManager;
        _productsPublicAppService = productsPublicAppService;
    }
    
    public async Task HandleEventAsync(OrderCreatedEto eventData)
    {
        await _productManager.RequestProductsAsync(new ProductsRequestedInput
        {
            Products = eventData.items.Select(x=> new KeyValuePair<Guid,int>(Guid.Parse(x.Key),x.Value)).ToDictionary(),
            RequesterId = eventData.OrderId.ToString()
        });
        
        await _paymentManager.CreateAsync(eventData.OrderId.ToString());
    }

    public async Task HandleEventAsync(ProductsRequestCompletedEto eventData)
    {
        await _orderLineManager.CreateAsync(Guid.Parse(eventData.RequesterId), eventData.Products.Select(x =>
            new OrderItemDto
            {
                Name = x.Key.Name,
                ItemId = x.Key.Id.ToString(),
                Amount = x.Value,
                Price = x.Key.Price
            }).ToList());
    }

    public async Task HandleEventAsync(OrderLinesProcessedDto eventData)
    {
        await _basketManager.RemoveItemsFromUserBasketAsync(Guid.Parse(eventData.UserId), eventData.Items.Select(x => new BasketItem
        {
            ItemId = x.ItemId,
            Amount = x.Amount
        }).ToList());
    }
}