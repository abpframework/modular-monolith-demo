using Shopularity.Ordering.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace Shopularity.Ordering.Orders
{
    public class OrderManager : DomainService
    {
        protected IOrderRepository _orderRepository;

        public OrderManager(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
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

    }
}