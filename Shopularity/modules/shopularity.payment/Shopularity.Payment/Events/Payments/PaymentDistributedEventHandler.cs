using System.Threading.Tasks;
using Shopularity.Ordering.Events.Orders;
using Shopularity.Payment.Domain.Payments;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Shopularity.Payment.Events.Payments;

public class PaymentDistributedEventHandler:
    IDistributedEventHandler<OrderCreatedEto>,
    IDistributedEventHandler<OrderCancelledEto>,
    ITransientDependency
{
    private readonly PaymentManager _paymentManager;

    public PaymentDistributedEventHandler(PaymentManager paymentManager)
    {
        _paymentManager = paymentManager;
    }


    public async Task HandleEventAsync(OrderCancelledEto eventData)
    {
        await _paymentManager.CancelAsync(eventData.OrderId);
    }

    public async Task HandleEventAsync(OrderCreatedEto eventData)
    {
        await _paymentManager.CreateAsync(eventData.Id, eventData.TotalPrice);
    }
}