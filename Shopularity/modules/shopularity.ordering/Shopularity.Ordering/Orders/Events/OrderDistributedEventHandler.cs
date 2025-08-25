using System;
using System.Threading.Tasks;
using Shopularity.Payment.Payments.Events;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Shopularity.Ordering.Orders.Events;

public class OrderDistributedEventHandler :
    IDistributedEventHandler<PaymentCompletedEto>,
    IDistributedEventHandler<PaymentCreatedEto>,
    IDistributedEventHandler<OrderShippedEto>,
    ITransientDependency
{
    private readonly OrderManager _orderManager;
    private readonly IBackgroundJobManager _backgroundJobManager;

    public OrderDistributedEventHandler(OrderManager orderManager, IBackgroundJobManager backgroundJobManager)
    {
        _orderManager = orderManager;
        _backgroundJobManager = backgroundJobManager;
    }

    public async Task HandleEventAsync(PaymentCompletedEto eventData)
    {
        await _orderManager.UpdateStateAsync(eventData.OrderId, OrderState.Paid);

        await _backgroundJobManager.EnqueueAsync(new OrderFakeStateJob.OrderFakeStateJobArgs
            {
                OrderId = eventData.OrderId,
                State = OrderState.Processing
            },
            delay: TimeSpan.FromSeconds(30));
    }

    public async Task HandleEventAsync(OrderShippedEto eventData)
    {
        await _backgroundJobManager.EnqueueAsync(new OrderFakeStateJob.OrderFakeStateJobArgs
            {
                OrderId = eventData.Id,
                State = OrderState.Completed
            },
            delay: TimeSpan.FromSeconds(60));
    }

    public async Task HandleEventAsync(PaymentCreatedEto eventData)
    {
        await _orderManager.UpdateStateAsync(eventData.OrderId, OrderState.WaitingForPayment);
    }
}