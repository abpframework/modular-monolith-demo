using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Shopularity.Catalog.Products.Admin;

public class ProductDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string Name { get; set; } = null!;
        
    public string? Description { get; set; }
        
    public double Price { get; set; }
        
    public int StockCount { get; set; }
        
    public Guid? CategoryId { get; set; }
        
    public byte[]? Image { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}