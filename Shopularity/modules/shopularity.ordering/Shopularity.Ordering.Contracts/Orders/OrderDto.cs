using Shopularity.Ordering.Orders;
using System;
using System.Collections.Generic;
using Shopularity.Ordering.OrderLines;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Shopularity.Ordering.Orders
{
    public class OrderDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string UserId { get; set; } = null!;
        public OrderState State { get; set; }
        public double TotalPrice { get; set; }
        public string ShippingAddress { get; set; } = null!;
        public string? CargoNo { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

        public List<OrderLineDto> OrderLines { get; set; } = new();
    }
}