using System.Threading.Tasks;
using Shopularity.Ordering.OrderLines;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Shopularity.Ordering.Orders.Events;

public class OrderLocalEventHandler : ILocalEventHandler<EntityDeletedEventData<Order>>, ITransientDependency
{
    private readonly IOrderLineRepository _orderLineRepository;

    public OrderLocalEventHandler(IOrderLineRepository orderLineRepository)
    {
        _orderLineRepository = orderLineRepository;

    }

    public async Task HandleEventAsync(EntityDeletedEventData<Order> eventData)
    {
        if (eventData.Entity is not ISoftDelete softDeletedEntity)
        {
            return;
        }

        if (!softDeletedEntity.IsDeleted)
        {
            return;
        }

        try
        {
            await _orderLineRepository.DeleteManyAsync(await _orderLineRepository.GetListByOrderIdAsync(eventData.Entity.Id));

        }
        catch
        {
            //...
        }
    }
}