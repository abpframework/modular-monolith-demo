using System;

namespace Shopularity.Basket.Services;

public class BasketItemRemovedEto
{
    public BasketItem Item {get; set; }
    
    public int RemainingItemCountInBasket { get; set; }
    
    public Guid UserId { get; set; }
}