using Shopularity.Payment.Payments;
using AutoMapper;

namespace Shopularity.Payment.Blazor;

public class PaymentBlazorAutoMapperProfile : Profile
{
    public PaymentBlazorAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<PaymentDto, PaymentUpdateDto>();
    }
}