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
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Order, OrderDto>();
        CreateMap<Order, OrderExcelDto>();

        CreateMap<OrderLine, OrderLineDto>();
    }
}