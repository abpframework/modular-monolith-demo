using AutoMapper;
using Shopularity.Catalog.Products.Public;
using Shopularity.Public.Pages;
using Volo.Abp.AutoMapper;

namespace Shopularity.Public.ObjectMapping;

public class ShopularityPublicAutoMapperProfile : Profile
{
    public ShopularityPublicAutoMapperProfile()
    {
        CreateMap<ProductPublicDto, CheckOutModel.BasketCacheItemModel>().Ignore(x=> x.Amount);
    }
}
