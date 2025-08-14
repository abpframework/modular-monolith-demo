using System.ComponentModel.DataAnnotations;

namespace Shopularity.Ordering.Orders.Public;

public class OrderCreatePublicDto
{
    public double TotalPrice { get; set; }
    
    [Required]
    [StringLength(OrderConsts.ShippingAddressMaxLength, MinimumLength = OrderConsts.ShippingAddressMinLength)]
    public string ShippingAddress { get; set; } = null!;
}