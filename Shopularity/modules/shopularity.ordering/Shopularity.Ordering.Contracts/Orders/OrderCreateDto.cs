using System;
using System.ComponentModel.DataAnnotations;

namespace Shopularity.Ordering.Orders;

public class OrderCreateDto
{
    [Required]
    public string UserId { get; set; } = null!;
    public OrderState State { get; set; } = ((OrderState[])Enum.GetValues(typeof(OrderState)))[0];
    public double TotalPrice { get; set; }
    [Required]
    [StringLength(OrderConsts.ShippingAddressMaxLength, MinimumLength = OrderConsts.ShippingAddressMinLength)]
    public string ShippingAddress { get; set; } = null!;
}