using System;
using System.Threading.Tasks;
using Shopularity.Payment.Payments.Events;
using Volo.Abp;
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
    private readonly IOrderRepository _orderRepository;
    private readonly IBackgroundJobManager _backgroundJobManager;

    public OrderDistributedEventHandler(
        IOrderRepository orderRepository,
        IBackgroundJobManager backgroundJobManager)
    {
        _orderRepository = orderRepository;
        _backgroundJobManager = backgroundJobManager;
    }

    public async Task HandleEventAsync(PaymentCompletedEto eventData)
    {
        var order = await _orderRepository.GetAsync(eventData.OrderId);
        order.State = OrderState.Paid;
        await _orderRepository.UpdateAsync(order);

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
        var order = await _orderRepository.GetAsync(eventData.OrderId);
        order.State = OrderState.WaitingForPayment;
        await _orderRepository.UpdateAsync(order);
    }
}