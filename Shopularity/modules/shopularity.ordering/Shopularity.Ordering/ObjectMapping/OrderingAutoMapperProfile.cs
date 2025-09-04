using AutoMapper;
using Shopularity.Ordering.Domain.Orders;
using Shopularity.Ordering.Domain.Orders.OrderLines;
using Shopularity.Ordering.Services.Orders.Admin;
using Shopularity.Ordering.Services.Orders.OrderLines;
using Shopularity.Ordering.Services.Orders.Public;
using Volo.Abp.AutoMapper;

namespace Shopularity.Ordering.ObjectMapping;

public class OrderingAutoMapperProfile : Profile
{
    public OrderingAutoMapperProfile()
    {
        CreateMap<Order, OrderDto>().Ignore(x=> x.Username);
        CreateMap<Order, OrderPublicDto>();
        CreateMap<Order, OrderExcelDto>().Ignore(x=> x.Username);

        CreateMap<OrderLine, OrderLineDto>();
    }
}