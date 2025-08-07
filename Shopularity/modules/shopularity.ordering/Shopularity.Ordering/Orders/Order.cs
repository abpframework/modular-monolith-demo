using Shopularity.Ordering.Orders;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Shopularity.Ordering.OrderLines;

using Volo.Abp;

namespace Shopularity.Ordering.Orders
{
    public class Order : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string UserId { get; set; }

        public virtual OrderState State { get; set; }

        public virtual double TotalPrice { get; set; }

        [NotNull]
        public virtual string ShippingAddress { get; set; }

        [CanBeNull]
        public virtual string? CargoNo { get; set; }

        public ICollection<OrderLine> OrderLines { get; private set; }

        protected Order()
        {

        }

        public Order(Guid id, string userId, OrderState state, double totalPrice, string shippingAddress)
        {

            Id = id;
            Check.NotNull(userId, nameof(userId));
            Check.NotNull(shippingAddress, nameof(shippingAddress));
            Check.Length(shippingAddress, nameof(shippingAddress), OrderConsts.ShippingAddressMaxLength, OrderConsts.ShippingAddressMinLength);
            UserId = userId;
            State = state;
            TotalPrice = totalPrice;
            ShippingAddress = shippingAddress;
            OrderLines = new Collection<OrderLine>();
        }

    }
}