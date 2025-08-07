using System;
using Shopularity.Catalog.Shared;
using Shopularity.Catalog.Products;
using Shopularity.Catalog.Categories;
using AutoMapper;
using Volo.Abp.AutoMapper;

namespace Shopularity.Catalog;

public class CatalogAutoMapperProfile : Profile
{
    public CatalogAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Category, CategoryDto>();

        CreateMap<Product, ProductDto>().Ignore(x=> x.Image);
        CreateMap<Product, ProductExcelDto>();
        CreateMap<ProductWithNavigationProperties, ProductWithNavigationPropertiesDto>();
        CreateMap<Category, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name));
    }
}