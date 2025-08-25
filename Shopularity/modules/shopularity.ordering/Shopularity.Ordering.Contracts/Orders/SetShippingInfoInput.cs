using System.ComponentModel.DataAnnotations;

namespace Shopularity.Ordering.Orders;

public class SetShippingInfoInput
{
    [Required]
    [StringLength(OrderConsts.CargoNoMaxLength, MinimumLength = OrderConsts.CargoNoMinLength)]
    public string CargoNo { get; set; }
}