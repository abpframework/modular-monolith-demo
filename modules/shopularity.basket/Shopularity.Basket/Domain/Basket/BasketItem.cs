using System;

namespace Shopularity.Basket.Domain.Basket;

public class BasketItem
{
    public Guid ItemId { get; set; }
    
    public int Amount { get; set; }
}