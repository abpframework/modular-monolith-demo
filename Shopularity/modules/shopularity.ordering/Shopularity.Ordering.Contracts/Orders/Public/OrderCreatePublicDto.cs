using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shopularity.Catalog.Services.Products;

namespace Shopularity.Ordering.Orders.Public;

public class OrderCreatePublicDto
{
    public List<ProductIdsWithAmountDto> Products { get; set; }
    
    [Required]
    [StringLength(OrderConsts.ShippingAddressMaxLength, MinimumLength = OrderConsts.ShippingAddressMinLength)]
    public string ShippingAddress { get; set; } = null!;
}