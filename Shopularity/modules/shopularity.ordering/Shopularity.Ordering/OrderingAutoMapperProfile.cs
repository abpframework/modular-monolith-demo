using Shopularity.Ordering.OrderLines;
using System;
using Shopularity.Ordering.Shared;
using Shopularity.Ordering.Orders;
using Volo.Abp.AutoMapper;
using AutoMapper;

namespace Shopularity.Ordering;

public class OrderingAutoMapperProfile : Profile
{
    public OrderingAutoMapperProfile()
    {
        CreateMap<Order, OrderDto>();
        CreateMap<Order, OrderExcelDto>();

        CreateMap<OrderLine, OrderLineDto>();
    }
}