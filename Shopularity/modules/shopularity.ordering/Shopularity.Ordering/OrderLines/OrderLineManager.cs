using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Shopularity.Ordering.OrderLines
{
    public class OrderLineManager : DomainService
    {
        protected IOrderLineRepository _orderLineRepository;

        public OrderLineManager(IOrderLineRepository orderLineRepository)
        {
            _orderLineRepository = orderLineRepository;
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