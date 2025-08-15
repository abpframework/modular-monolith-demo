using System;
using System.Collections.Generic;

namespace Shopularity.Catalog.Products.Events;

public class ProductsRequestedEto
{
    public Dictionary<Guid, int> Products { get; set; }
    
    public string RequesterId { get; set; }
}