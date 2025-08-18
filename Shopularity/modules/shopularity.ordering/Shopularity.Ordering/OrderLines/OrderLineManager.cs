using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopularity.Ordering.Orders;
using Shopularity.Ordering.Orders.Events;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;

namespace Shopularity.Ordering.OrderLines
{
    public class OrderLineManager : DomainService
    {
        protected IOrderLineRepository _orderLineRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly OrderManager _orderManager;
        private readonly IDistributedEventBus _eventBus;

        public OrderLineManager(IOrderLineRepository orderLineRepository, IOrderRepository orderRepository, OrderManager orderManager, IDistributedEventBus eventBus)
        {
            _orderLineRepository = orderLineRepository;
            _orderRepository = orderRepository;
            _orderManager = orderManager;
            _eventBus = eventBus;
        }

        public virtual async Task CreateAsync(Guid orderId, List<OrderItemDto> items)
        {        
            var order = await _orderRepository.GetAsync(orderId);
            var totalOrderPrice = items.Select(x => x.Amount * x.Price).Sum();
            await _orderManager.UpdatePriceAsync(order.Id, totalOrderPrice);
        
            foreach (var eventDataItem in items)
            {
                var item = eventDataItem;
                var totalPrice = item.Price * item.Amount;

                await CreateAsync(order.Id, item.ItemId, item.Price, item.Amount, totalPrice, item.Name);
            }

            await _eventBus.PublishAsync(
                new OrderLinesProcessedDto
                {
                    Items = items,
                    UserId = order.UserId
                }
            );
        }

        public virtual async Task<OrderLine> CreateAsync(Guid orderId, string productId, double price, int amount, double totalPrice, string? name = null)
        {
            Check.NotNullOrWhiteSpace(productId, nameof(productId));
            Check.Length(name, nameof(name), OrderLineConsts.NameMaxLength);

            var orderLine = new OrderLine(
             GuidGenerator.Create(),
             orderId, productId, price, amount, totalPrice, name
             );

            return await _orderLineRepository.InsertAsync(orderLine);
        }

        public virtual async Task<OrderLine> UpdateAsync(Guid id, Guid orderId, string? name = null)
        {
            Check.Length(name, nameof(name), OrderLineConsts.NameMaxLength);

            var orderLine = await _orderLineRepository.GetAsync(id);

            orderLine.OrderId = orderId;
            orderLine.Name = name;

            return await _orderLineRepository.UpdateAsync(orderLine);
        }
    }
}