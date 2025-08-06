using Shopularity.Payment.Localization;
using Volo.Abp.Application.Services;

namespace Shopularity.Payment;

public abstract class PaymentAppService : ApplicationService
{
    protected PaymentAppService()
    {
        LocalizationResource = typeof(PaymentResource);
        ObjectMapperContext = typeof(PaymentModule);
    }
}
