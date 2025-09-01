using System;
using Volo.Abp.Application.Dtos;

namespace Shopularity.Ordering.OrderLines;

public class OrderLineDto : FullAuditedEntityDto<Guid>
{
    public Guid OrderId { get; set; }
    public string ProductId { get; set; } = null!;
    public string? Name { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }
    public double TotalPrice { get; set; }

}