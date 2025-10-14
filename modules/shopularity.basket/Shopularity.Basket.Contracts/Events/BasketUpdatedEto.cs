using System;

namespace Shopularity.Basket.Events;

public class BasketUpdatedEto
{
    public Guid UserId { get; set; }
    
    public int ItemCountInBasket { get; set; }
}