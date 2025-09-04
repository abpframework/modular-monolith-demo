using System;

namespace Shopularity.Basket.Services;

public class RemoveBasketItemInput
{
    public Guid ItemId { get; set; }
    
    public int Amount { get; set; }
}