using System;

namespace Shopularity.Basket.Services;

public class BasketItem
{
    public Guid ProductId { get; set; }
    
    public int Amount { get; set; }
}