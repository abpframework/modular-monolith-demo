using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Shopularity.Payment.Payments
{
    public class PaymentDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string OrderId { get; set; } = null!;
        public PaymentState State { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

    }
}