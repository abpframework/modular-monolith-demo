using System;
using Shopularity.Payment.Domain.Payments;

namespace Shopularity.Payment.Services.Payments;

public class PaymentExcelDownloadDto
{
    public string DownloadToken { get; set; } = null!;
    
    public Guid? OrderId { get; set; }
    
    public PaymentState? State { get; set; }
}