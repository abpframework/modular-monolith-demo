using System;

namespace Shopularity.Basket.Services;

public class AddBasketItemInput
{
    public Guid ItemId { get; set; }
    
    public int Amount { get; set; }
}