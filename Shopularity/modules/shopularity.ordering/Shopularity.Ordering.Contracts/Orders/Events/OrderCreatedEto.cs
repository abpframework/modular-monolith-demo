using System;
using System.Collections.Generic;

namespace Shopularity.Ordering.Orders.Events;

public class OrderCreatedEto
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public List<ProductWithAmountDto> Products { get; set; }
}