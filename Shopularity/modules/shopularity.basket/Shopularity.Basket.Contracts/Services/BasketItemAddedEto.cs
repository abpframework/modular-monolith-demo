using System;
using System.Collections.Generic;

namespace Shopularity.Basket.Services;

public class BasketItemAddedEto
{
    public BasketItem Item {get; set; }
    
    public int RemainingItemCountInBasket { get; set; }
    
    public Guid UserId { get; set; }
}