using System;

namespace Shopularity.Payment.Services.Payments;

[Serializable]
public class PaymentDownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}