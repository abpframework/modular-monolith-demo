using System;

namespace Shopularity.Ordering.Orders.Public;

public class ProductIdsWithAmountDto
{
    public Guid ProductId { get; set; }
    
    public int Amount { get; set; }
}