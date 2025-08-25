using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace Shopularity.Ordering.Orders.Events;

public class OrderFakeStateJob : AsyncBackgroundJob<OrderFakeStateJob.OrderFakeStateJobArgs>, ITransientDependency
{
    private readonly OrderManager _orderManager;

    public OrderFakeStateJob(OrderManager orderManager)
    {
        _orderManager = orderManager;
    }

    public override async Task ExecuteAsync(OrderFakeStateJobArgs args)
    {
        await _orderManager.UpdateStateAsync(args.OrderId, OrderState.Processing);
    }

    public class OrderFakeStateJobArgs
    {
        public Guid OrderId { get; set; }
    }
}