using System;
using Shopularity.Payment.Domain.Payments;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Shopularity.Payment.Services.Payments;

public class PaymentDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string OrderId { get; set; } = null!;

    public double TotalPrice { get; set; }
    
    public PaymentState State { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}