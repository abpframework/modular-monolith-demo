using System;
using System.Collections.Generic;

namespace Shopularity.Basket.Services;

public class BasketChangedEto
{
    public List<BasketItem> Items {get; set; }
    
    public Guid UserId { get; set; }
}