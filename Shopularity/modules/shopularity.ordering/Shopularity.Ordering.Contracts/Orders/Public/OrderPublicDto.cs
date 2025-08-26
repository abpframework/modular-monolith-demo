using System;
using System.Collections.Generic;
using Shopularity.Ordering.OrderLines;

namespace Shopularity.Ordering.Orders.Public;

public class OrderPublicDto
{
    public Guid Id { get; set; }
    
    public OrderState State { get; set; }
    
    public double TotalPrice { get; set; }
    
    public string ShippingAddress { get; set; } = null!;
    
    public string? CargoNo { get; set; }
    
    public DateTime CreationTime { get; set; }

    public List<OrderLineDto> OrderLines { get; set; } = new();
}