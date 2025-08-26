using AutoMapper;
using Shopularity.Catalog.Products.Public;
using Shopularity.Public.Components.Basket;
using Volo.Abp.AutoMapper;

namespace Shopularity.Public.ObjectMapping;

public class ShopularityPublicAutoMapperProfile : Profile
{
    public ShopularityPublicAutoMapperProfile()
    {
        CreateMap<ProductPublicDto, BasketViewItemModel>().Ignore(x=> x.Amount);
    }
}
