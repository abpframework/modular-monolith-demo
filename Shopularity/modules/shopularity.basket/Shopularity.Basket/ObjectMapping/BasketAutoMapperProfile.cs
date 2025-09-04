using AutoMapper;
using Shopularity.Basket.Domain.Basket;

namespace Shopularity.Basket.ObjectMapping;

public class BasketAutoMapperProfile : Profile
{
    public BasketAutoMapperProfile()
    {
        CreateMap<BasketItem, BasketItemWithProductInfo>();
    }
}