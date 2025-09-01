using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Shopularity.Ordering.Orders;

public class OrderUpdateDto : IHasConcurrencyStamp
{
    [Required]
    [StringLength(OrderConsts.ShippingAddressMaxLength, MinimumLength = OrderConsts.ShippingAddressMinLength)]
    public string ShippingAddress { get; set; } = null!;

    public string ConcurrencyStamp { get; set; } = null!;
}