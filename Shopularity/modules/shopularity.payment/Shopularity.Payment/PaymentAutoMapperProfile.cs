using Shopularity.Payment.Payments;
using AutoMapper;

namespace Shopularity.Payment;

public class PaymentAutoMapperProfile : Profile
{
    public PaymentAutoMapperProfile()
    {
        CreateMap<Payments.Payment, PaymentDto>();
        CreateMap<Payments.Payment, PaymentExcelDto>();
    }
}