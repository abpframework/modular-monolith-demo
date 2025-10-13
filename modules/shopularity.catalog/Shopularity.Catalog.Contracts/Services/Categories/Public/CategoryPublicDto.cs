using System;

namespace Shopularity.Catalog.Services.Categories.Public;

public class CategoryPublicDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
}