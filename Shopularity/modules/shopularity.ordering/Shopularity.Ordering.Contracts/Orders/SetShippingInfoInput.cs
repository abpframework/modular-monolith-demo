using System.ComponentModel.DataAnnotations;

namespace Shopularity.Ordering.Orders;

public class SetShippingInfoInput
{
    [StringLength(OrderConsts.CargoNoMaxLength, MinimumLength = OrderConsts.CargoNoMinLength)]
    public string? CargoNo { get; set; }
}