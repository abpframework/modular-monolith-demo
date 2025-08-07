using Shopularity.Ordering.OrderLines;
using Volo.Abp.AutoMapper;
using Shopularity.Ordering.Orders;
using AutoMapper;

namespace Shopularity.Ordering.Blazor;

public class OrderingBlazorAutoMapperProfile : Profile
{
    public OrderingBlazorAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<OrderDto, OrderUpdateDto>();

        CreateMap<OrderLineDto, OrderLineUpdateDto>();
    }
}