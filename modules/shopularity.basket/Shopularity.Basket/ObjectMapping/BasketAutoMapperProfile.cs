using AutoMapper;
using Shopularity.Basket.Domain.Basket;
using Shopularity.Basket.Services;

namespace Shopularity.Basket.ObjectMapping;

public class BasketAutoMapperProfile : Profile
{
    public BasketAutoMapperProfile()
    {
        CreateMap<BasketItemWithProductInfo, BasketItemDto>();
    }
}