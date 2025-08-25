using System;

namespace Shopularity.Ordering.Orders.Events;

public class OrderShippedEto
{
    public Guid Id { get; set; }
    
    public string Address { get; set; }
    
    public string CargoNo { get; set; }
}