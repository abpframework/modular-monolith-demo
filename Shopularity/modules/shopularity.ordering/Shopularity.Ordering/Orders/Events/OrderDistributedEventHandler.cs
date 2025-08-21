using System;
using System.Threading.Tasks;
using Shopularity.Payment.Payments.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Shopularity.Ordering.Orders.Events;

public class OrderDistributedEventHandler : IDistributedEventHandler<PaymentCompletedEto>, ITransientDependency
{
    private readonly OrderManager _orderManager;
    private readonly OrderFakeStateService _orderFakeStateService;

    public OrderDistributedEventHandler(OrderManager orderManager, OrderFakeStateService orderFakeStateService)
    {
        _orderManager = orderManager;
        _orderFakeStateService = orderFakeStateService;
    }
    
    public async Task HandleEventAsync(PaymentCompletedEto eventData)
    {
        await _orderManager.UpdateStateAsync(Guid.Parse(eventData.OrderId), OrderState.Paid);
        
        await _orderFakeStateService.FakeOrderProcessingAsync(Guid.Parse(eventData.OrderId));
    }
}