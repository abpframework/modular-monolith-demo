using System;
using System.ComponentModel.DataAnnotations;

namespace Shopularity.Payment.Payments
{
    public class PaymentCreateDto
    {
        [Required]
        public string OrderId { get; set; } = null!;
        public PaymentState State { get; set; } = ((PaymentState[])Enum.GetValues(typeof(PaymentState)))[0];
    }
}