using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Shopularity.Ordering.Orders.Events;

public class OrderFakeStateService : ISingletonDependency
{
    private readonly OrderManager _orderManager;

    public OrderFakeStateService(OrderManager orderManager)
    {
        _orderManager = orderManager;
    }

    public async Task FakeOrderProcessingAsync(Guid id)
    {
        _ = Task.Run(async () =>
        {
            await Task.Delay(1000 * 60);

            await _orderManager.UpdateStateAsync(id, OrderState.Processing);
        });
    }
}