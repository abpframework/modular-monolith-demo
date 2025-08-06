using System;

namespace Shopularity.Payment.Payments;

[Serializable]
public class PaymentDownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}