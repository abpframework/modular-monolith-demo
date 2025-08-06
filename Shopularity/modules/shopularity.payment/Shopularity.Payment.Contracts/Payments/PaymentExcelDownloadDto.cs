namespace Shopularity.Payment.Payments
{
    public class PaymentExcelDownloadDto
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public string? OrderId { get; set; }
        public PaymentState? State { get; set; }

        public PaymentExcelDownloadDto()
        {

        }
    }
}