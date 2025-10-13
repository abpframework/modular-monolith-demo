using System.ComponentModel.DataAnnotations;
using Shopularity.Ordering.Domain.Orders;
using Volo.Abp.Domain.Entities;

namespace Shopularity.Ordering.Services.Orders.Admin;

public class OrderUpdateDto : IHasConcurrencyStamp
{
    [Required]
    [StringLength(OrderConsts.ShippingAddressMaxLength, MinimumLength = OrderConsts.ShippingAddressMinLength)]
    public string ShippingAddress { get; set; } = null!;

    public string ConcurrencyStamp { get; set; } = null!;
}