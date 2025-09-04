using System.ComponentModel.DataAnnotations;
using Shopularity.Ordering.Domain.Orders;

namespace Shopularity.Ordering.Services.Orders.Admin;

public class SetShippingInfoInput
{
    [Required]
    [StringLength(OrderConsts.CargoNoMaxLength, MinimumLength = OrderConsts.CargoNoMinLength)]
    public string CargoNo { get; set; }
}