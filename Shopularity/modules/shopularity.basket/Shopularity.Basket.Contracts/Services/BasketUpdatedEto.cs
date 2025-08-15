using System;
using System.Collections.Generic;

namespace Shopularity.Basket.Services;

public class BasketUpdatedEto
{
    public int ItemCountInBasket { get; set; }
    
    public Guid UserId { get; set; }
}