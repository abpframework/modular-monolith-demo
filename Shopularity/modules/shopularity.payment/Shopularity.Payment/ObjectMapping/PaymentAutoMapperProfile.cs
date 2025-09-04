using AutoMapper;
using Shopularity.Payment.Services.Payments;

namespace Shopularity.Payment.ObjectMapping;

public class PaymentAutoMapperProfile : Profile
{
    public PaymentAutoMapperProfile()
    {
        CreateMap<Domain.Payments.Payment, PaymentDto>();
        CreateMap<Domain.Payments.Payment, PaymentExcelDto>();
    }
}