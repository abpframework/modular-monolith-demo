using System;

namespace Shopularity.Payment.Payments
{
    public class PaymentExcelDownloadDto
    {
        public string DownloadToken { get; set; } = null!;
        public Guid? OrderId { get; set; }
        public PaymentState? State { get; set; }

        public PaymentExcelDownloadDto()
        {

        }
    }
}