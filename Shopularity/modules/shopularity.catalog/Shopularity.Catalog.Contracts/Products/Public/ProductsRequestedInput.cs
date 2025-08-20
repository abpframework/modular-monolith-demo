using System;
using System.Collections.Generic;

namespace Shopularity.Catalog.Products.Public;

public class ProductsRequestedInput
{
    public Dictionary<Guid, int> Products { get; set; }
    
    public string RequesterId { get; set; }
}