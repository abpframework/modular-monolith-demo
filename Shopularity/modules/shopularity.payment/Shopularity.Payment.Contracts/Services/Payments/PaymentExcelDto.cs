using Shopularity.Payment.Domain.Payments;

namespace Shopularity.Payment.Services.Payments;

public class PaymentExcelDto
{
    public string OrderId { get; set; } = null!;
    public PaymentState State { get; set; }
}