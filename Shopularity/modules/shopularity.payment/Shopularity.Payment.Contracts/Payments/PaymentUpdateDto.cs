using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Shopularity.Payment.Payments
{
    public class PaymentUpdateDto : IHasConcurrencyStamp
    {
        [Required]
        public string OrderId { get; set; } = null!;
        public PaymentState State { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
    }
}