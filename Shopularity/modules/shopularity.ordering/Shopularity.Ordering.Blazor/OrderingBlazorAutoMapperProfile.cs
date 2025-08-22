using Shopularity.Ordering.OrderLines;
using Volo.Abp.AutoMapper;
using Shopularity.Ordering.Orders;
using AutoMapper;

namespace Shopularity.Ordering.Blazor;

public class OrderingBlazorAutoMapperProfile : Profile
{
    public OrderingBlazorAutoMapperProfile()
    {
        CreateMap<OrderDto, OrderUpdateDto>();

        CreateMap<OrderDto, SetShippingInfoInput>();

        CreateMap<OrderLineDto, OrderLineUpdateDto>();
    }
}