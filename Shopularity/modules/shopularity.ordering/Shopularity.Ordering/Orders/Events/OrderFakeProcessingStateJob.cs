using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace Shopularity.Ordering.Orders.Events;

public class OrderFakeStateJob : AsyncBackgroundJob<OrderFakeStateJob.OrderFakeStateJobArgs>, ITransientDependency
{
    private readonly IOrderRepository _orderRepository;

    public OrderFakeStateJob(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public override async Task ExecuteAsync(OrderFakeStateJobArgs args)
    {
        var order = await _orderRepository.GetAsync(args.OrderId);
        order.State = args.State;
        await _orderRepository.UpdateAsync(order);
    }

    public class OrderFakeStateJobArgs
    {
        public Guid OrderId { get; set; }
        
        public OrderState State { get; set; }
    }
}