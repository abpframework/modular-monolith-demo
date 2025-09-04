using System;
using System.ComponentModel.DataAnnotations;

namespace Shopularity.Catalog.Services.Products.Admin;

public class ProductCreateDto
{
    [Required]
    [StringLength(ProductConsts.NameMaxLength, MinimumLength = ProductConsts.NameMinLength)]
    public string Name { get; set; } = null!;
        
    [StringLength(ProductConsts.DescriptionMaxLength, MinimumLength = ProductConsts.DescriptionMinLength)]
    public string? Description { get; set; }
        
    public double Price { get; set; }
        
    public int StockCount { get; set; }
        
    public byte[]? Image { get; set; }
        
    public Guid? CategoryId { get; set; }
}