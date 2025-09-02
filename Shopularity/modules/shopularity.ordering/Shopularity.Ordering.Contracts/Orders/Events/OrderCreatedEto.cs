using System;
using System.Collections.Generic;

namespace Shopularity.Ordering.Orders.Events;

public class OrderCreatedEto
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public double TotalPrice { get; set; }
    
    public Dictionary<Guid, int> ProductsWithAmounts { get; set; }
}