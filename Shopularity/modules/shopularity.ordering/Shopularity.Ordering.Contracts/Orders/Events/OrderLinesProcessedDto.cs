using System.Collections.Generic;

namespace Shopularity.Ordering.Orders.Events;

public class OrderLinesProcessedDto
{
    public List<OrderItemDto> Items { get; set; }
    
    public string UserId { get; set; }
}