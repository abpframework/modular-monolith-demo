using System.Collections.Generic;
using Shopularity.Catalog.Products.Admin;

namespace Shopularity.Catalog.Products.Events;

public class ProductsRequestCompletedEto
{
    public List<KeyValuePair<ProductDto, int>> Products { get; set; }
    
    public string RequesterId { get; set; }
}