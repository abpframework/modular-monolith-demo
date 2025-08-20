using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shopularity.Catalog.Products.Admin;

namespace Shopularity.Ordering.Orders.Public;

public class OrderCreatePublicDto
{
    public List<OrderCreatePublicProductDto> Products { get; set; }
    
    [Required]
    [StringLength(OrderConsts.ShippingAddressMaxLength, MinimumLength = OrderConsts.ShippingAddressMinLength)]
    public string ShippingAddress { get; set; } = null!;
}

public class OrderCreatePublicProductDto
{
    public Guid ProductId { get; set; }
    
    public int Amount { get; set; }
}