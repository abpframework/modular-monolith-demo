using Shopularity.Ordering.OrderLines;
using Shopularity.Ordering.Orders;
using Volo.Abp.AutoMapper;
using AutoMapper;
using Shopularity.Ordering.Orders.Public;

namespace Shopularity.Ordering;

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