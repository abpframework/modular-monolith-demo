using Shopularity.Ordering.Orders.Events;
using Shopularity.Payment.Payments;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Shopularity.Events;

public class ShopularityEventHandler:
    IDistributedEventHandler<OrderCreatedEto>,
    ITransientDependency
{
    private readonly PaymentManager _paymentManager;

    public ShopularityEventHandler(PaymentManager paymentManager)
    {
        _paymentManager = paymentManager;
    }
    
    public async Task HandleEventAsync(OrderCreatedEto eventData)
    {
        await _paymentManager.CreateAsync(eventData.Id.ToString());
    }
}