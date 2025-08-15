using System;
using System.Collections.Generic;

namespace Shopularity.Ordering.Orders.Events;

public class OrderCreatedEto
{
    public Dictionary<string, int> items { get; set; }
    
    public Guid OrderId { get; set; }
}