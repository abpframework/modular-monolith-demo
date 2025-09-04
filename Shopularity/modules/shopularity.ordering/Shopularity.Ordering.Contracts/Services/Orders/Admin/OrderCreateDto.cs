using System;
using System.ComponentModel.DataAnnotations;
using Shopularity.Ordering.Domain.Orders;

namespace Shopularity.Ordering.Services.Orders.Admin;

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