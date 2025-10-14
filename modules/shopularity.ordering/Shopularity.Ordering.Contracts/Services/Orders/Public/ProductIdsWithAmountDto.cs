using System;

namespace Shopularity.Ordering.Services.Orders.Public;

public class ProductIdsWithAmountDto
{
    public Guid ProductId { get; set; }
    
    public int Amount { get; set; }
}