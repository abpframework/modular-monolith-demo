using System;
using AutoMapper;
using Shopularity.Catalog.Domain.Categories;
using Shopularity.Catalog.Domain.Products;
using Shopularity.Catalog.Services.Categories.Admin;
using Shopularity.Catalog.Services.Categories.Public;
using Shopularity.Catalog.Services.Products.Admin;
using Shopularity.Catalog.Services.Products.Public;
using Shopularity.Catalog.Shared;
using Volo.Abp.AutoMapper;

namespace Shopularity.Catalog.ObjectMapping;

public class CatalogAutoMapperProfile : Profile
{
    public CatalogAutoMapperProfile()
    {
        CreateMap<Category, CategoryDto>();
        CreateMap<Category, CategoryPublicDto>();

        CreateMap<Product, ProductDto>().Ignore(x=> x.Image);
        CreateMap<Product, ProductPublicDto>();
        CreateMap<Product, ProductExcelDto>();
        CreateMap<ProductWithNavigationProperties, ProductWithNavigationPropertiesDto>();
        CreateMap<ProductWithNavigationProperties, ProductWithNavigationPropertiesPublicDto>();
        CreateMap<Category, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name));
    }
}