using System;
using Shopularity.Catalog.Shared;
using Shopularity.Catalog.Products;
using Shopularity.Catalog.Categories;
using AutoMapper;
using Shopularity.Catalog.Categories.Public;
using Shopularity.Catalog.Products.Admin;
using Shopularity.Catalog.Products.Public;
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
        CreateMap<Category, CategoryPublicDto>();

        CreateMap<Product, ProductDto>().Ignore(x=> x.Image);
        CreateMap<Product, ProductPublicDto>().Ignore(x=> x.Image);
        CreateMap<Product, ProductExcelDto>();
        CreateMap<ProductWithNavigationProperties, ProductWithNavigationPropertiesDto>();
        CreateMap<ProductWithNavigationProperties, ProductWithNavigationPropertiesPublicDto>();
        CreateMap<Category, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name));
    }
}