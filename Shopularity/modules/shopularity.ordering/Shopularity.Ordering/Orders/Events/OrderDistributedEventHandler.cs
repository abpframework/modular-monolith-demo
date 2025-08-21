using System;
using System.Threading.Tasks;
using Shopularity.Payment.Payments.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Shopularity.Ordering.Orders.Events;

public class OrderDistributedEventHandler : IDistributedEventHandler<PaymentCompletedEto>, ITransientDependency
{
    private readonly OrderManager _orderManager;

    public OrderDistributedEventHandler(OrderManager orderManager)
    {
        _orderManager = orderManager;
    }
    
    public async Task HandleEventAsync(PaymentCompletedEto eventData)
    {
        await _orderManager.UpdateStateAsync(Guid.Parse(eventData.OrderId), OrderState.Paid);
        
        _ = Task.Run(async () =>
        {
            await FakeOrderProcessingAsync(Guid.Parse(eventData.OrderId));
        });
    }

    private async Task FakeOrderProcessingAsync(Guid id)
    {
        await Task.Delay(1000 * 10);

        await _orderManager.UpdateStateAsync(id, OrderState.Processing);
    }
}