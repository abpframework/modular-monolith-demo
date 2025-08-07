using Shopularity.Ordering.Orders;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Shopularity.Ordering.Orders
{
    public class OrderUpdateDto : IHasConcurrencyStamp
    {
        [Required]
        public string UserId { get; set; } = null!;
        public OrderState State { get; set; }
        public double TotalPrice { get; set; }
        [Required]
        [StringLength(OrderConsts.ShippingAddressMaxLength, MinimumLength = OrderConsts.ShippingAddressMinLength)]
        public string ShippingAddress { get; set; } = null!;
        [StringLength(OrderConsts.CargoNoMaxLength, MinimumLength = OrderConsts.CargoNoMinLength)]
        public string? CargoNo { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
    }
}