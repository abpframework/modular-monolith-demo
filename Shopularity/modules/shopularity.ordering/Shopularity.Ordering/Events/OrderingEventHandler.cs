using System.Linq;
using System.Threading.Tasks;
using Shopularity.Ordering.OrderLines;
using Shopularity.Ordering.Orders;
using Shopularity.Ordering.Orders.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;

namespace Shopularity.Ordering.Events;

public class OrderingEventHandler : IDistributedEventHandler<OrderLinesReceivedEto>
{
    private readonly IDistributedEventBus _eventBus;
    private readonly IOrderRepository _orderRepository;
    private readonly OrderManager _orderManager;
    private readonly OrderLineManager _orderLineManager;

    public OrderingEventHandler(
        IDistributedEventBus eventBus,
        IOrderRepository orderRepository,
        OrderManager orderManager,
        OrderLineManager orderLineManager)
    {
        _eventBus = eventBus;
        _orderRepository = orderRepository;
        _orderManager = orderManager;
        _orderLineManager = orderLineManager;
    }
    
    public async Task HandleEventAsync(OrderLinesReceivedEto eventData)
    {
        var order = await _orderRepository.GetAsync(eventData.OrderId);
        var totalOrderPrice = eventData.Items.Select(x => x.Amount * x.Price).Sum();
        await _orderManager.UpdatePriceAsync(order.Id, totalOrderPrice);
        
        foreach (var eventDataItem in eventData.Items)
        {
            var item = eventDataItem;
            var totalPrice = item.Price * item.Amount;

            await _orderLineManager.CreateAsync(order.Id, item.ItemId, item.Price, item.Amount, totalPrice, item.Name);
        }

        await _eventBus.PublishAsync(
                new OrderLinesProcessedDto
                {
                    Items = eventData.Items,
                    UserId = order.UserId
                }
            );
    }
}