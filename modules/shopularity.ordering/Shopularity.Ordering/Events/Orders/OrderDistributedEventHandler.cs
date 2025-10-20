using System;
using System.Threading.Tasks;
using Shopularity.Ordering.Domain.Orders;
using Shopularity.Payment.Events.Payments;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Shopularity.Ordering.Events.Orders;

public class OrderDistributedEventHandler :
    IDistributedEventHandler<PaymentCompletedEto>,
    IDistributedEventHandler<PaymentCreatedEto>,
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
        order.SetState(OrderState.Paid);
        await _orderRepository.UpdateAsync(order);

        await _backgroundJobManager.EnqueueAsync(new OrderFakeStateJob.OrderFakeStateJobArgs
            {
                OrderId = eventData.OrderId,
                State = OrderState.Processing
            },
            delay: TimeSpan.FromSeconds(30));
    }

    public async Task HandleEventAsync(PaymentCreatedEto eventData)
    {
        var order = await _orderRepository.GetAsync(eventData.OrderId);
        order.SetState(OrderState.WaitingForPayment);
        await _orderRepository.UpdateAsync(order);
    }
}