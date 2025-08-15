using System;
using System.Collections.Generic;

namespace Shopularity.Ordering.Orders.Events;

public class OrderLinesReceivedEto
{
    public List<OrderItemDto> Items { get; set; }
    
    public Guid OrderId { get; set; }
}

public class OrderItemDto
{
    public virtual string ItemId { get; set; }

    public virtual string Name { get; set; }

    public virtual double Price { get; set; }

    public virtual int Amount { get; set; }
}