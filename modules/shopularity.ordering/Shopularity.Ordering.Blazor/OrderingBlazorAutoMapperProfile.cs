using AutoMapper;
using Shopularity.Ordering.Services.Orders.Admin;

namespace Shopularity.Ordering.Blazor;

public class OrderingBlazorAutoMapperProfile : Profile
{
    public OrderingBlazorAutoMapperProfile()
    {
        CreateMap<OrderDto, OrderUpdateDto>();

        CreateMap<OrderDto, SetShipmentCargoNoInput>();
    }
}