using Shopularity.Ordering.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Shopularity.Ordering.Orders.Events;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace Shopularity.Ordering.Orders
{
    public class OrderManager : DomainService
    {
        protected IOrderRepository _orderRepository;
        private readonly IDistributedEventBus _eventBus;
        private readonly ICurrentUser _currentUser;

        public OrderManager(IOrderRepository orderRepository, IDistributedEventBus eventBus, ICurrentUser currentUser)
        {
            _orderRepository = orderRepository;
            _eventBus = eventBus;
            _currentUser = currentUser;
        }

        public virtual async Task<Order> CreateNewAsync(
        string userId,
        string shippingAddress,
        Dictionary<string, int> items)
        {
            Check.NotNullOrWhiteSpace(userId, nameof(userId));
            Check.NotNullOrWhiteSpace(shippingAddress, nameof(shippingAddress));
            Check.Length(shippingAddress, nameof(shippingAddress), OrderConsts.ShippingAddressMaxLength);

            var order = new Order(
             GuidGenerator.Create(),
             userId, 
             OrderState.New, 
             0, 
             shippingAddress
             );

            order = await _orderRepository.InsertAsync(order);

            await _eventBus.PublishAsync(
                new OrderCreatedEto
                {
                    items = items,
                    OrderId = order.Id
                }
            );

            return order;
        }

        public virtual async Task<Order> CreateAsync(
        string userId, OrderState state, double totalPrice, string shippingAddress)
        {
            Check.NotNullOrWhiteSpace(userId, nameof(userId));
            Check.NotNull(state, nameof(state));
            Check.NotNullOrWhiteSpace(shippingAddress, nameof(shippingAddress));
            Check.Length(shippingAddress, nameof(shippingAddress), OrderConsts.ShippingAddressMaxLength, OrderConsts.ShippingAddressMinLength);

            var order = new Order(
             GuidGenerator.Create(),
             userId, state, totalPrice, shippingAddress
             );

            return await _orderRepository.InsertAsync(order);
        }

        public virtual async Task CancelAsync(Guid id)
        {
            var order = await _orderRepository.GetAsync(id);

            if (order.UserId != _currentUser.GetId().ToString())
            {
                // todo: business exception
                throw new UserFriendlyException("Can't cancel someone else's order!!");
            }

            if (order.State is OrderState.Shipped or OrderState.Completed)
            {
                // todo: business exception
                throw new UserFriendlyException("Can't cancel shipped or completed orders!!");
            }
            
            order.State = OrderState.Cancelled;

            await _orderRepository.UpdateAsync(order);

            await _eventBus.PublishAsync(new OrderCancelledEto
            {
                Id = order.Id
            });
        }

        public virtual async Task<Order> UpdateAsync(
            Guid id,
            OrderState state, string shippingAddress, string? cargoNo = null, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNull(state, nameof(state));
            Check.NotNullOrWhiteSpace(shippingAddress, nameof(shippingAddress));
            Check.Length(shippingAddress, nameof(shippingAddress), OrderConsts.ShippingAddressMaxLength, OrderConsts.ShippingAddressMinLength);
            Check.Length(cargoNo, nameof(cargoNo), OrderConsts.CargoNoMaxLength, OrderConsts.CargoNoMinLength);

            var order = await _orderRepository.GetAsync(id);

            order.State = state;
            order.ShippingAddress = shippingAddress;
            order.CargoNo = cargoNo;

            order.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _orderRepository.UpdateAsync(order);
        }

        public async Task UpdatePriceAsync(Guid id, double totalOrderPrice)
        {
            var order = await _orderRepository.GetAsync(id);

            order.TotalPrice = totalOrderPrice;

            await _orderRepository.UpdateAsync(order);
        }
    }
}