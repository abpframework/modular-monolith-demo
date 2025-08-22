namespace Shopularity.Payment.Payments;

public class PaymentExcelDto
{
    public string OrderId { get; set; } = null!;
    public PaymentState State { get; set; }
}