using System;
using System.Collections.Generic;

namespace Shopularity.Catalog.Products;

public class GetListByIdsInput
{
    public List<Guid> Ids { get; set; } = new List<Guid>();
}