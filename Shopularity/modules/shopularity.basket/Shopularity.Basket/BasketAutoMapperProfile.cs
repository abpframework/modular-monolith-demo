using AutoMapper;
using Shopularity.Basket.Domain;

namespace Shopularity.Basket;

public class BasketAutoMapperProfile : Profile
{
    public BasketAutoMapperProfile()
    {
        CreateMap<BasketItem, BasketItemWithProductInfo>();
    }
}