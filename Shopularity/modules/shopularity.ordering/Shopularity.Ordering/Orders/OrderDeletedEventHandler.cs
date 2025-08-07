using Shopularity.Ordering.OrderLines;

using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Shopularity.Ordering.Orders;

public class OrderDeletedEventHandler : ILocalEventHandler<EntityDeletedEventData<Order>>, ITransientDependency
{
    private readonly IOrderLineRepository _orderLineRepository;

    public OrderDeletedEventHandler(IOrderLineRepository orderLineRepository)
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