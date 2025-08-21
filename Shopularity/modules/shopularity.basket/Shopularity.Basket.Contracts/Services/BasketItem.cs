using System;

namespace Shopularity.Basket.Services;

public class BasketItem
{
    public Guid ItemId { get; set; }
    
    public int Amount { get; set; }
}