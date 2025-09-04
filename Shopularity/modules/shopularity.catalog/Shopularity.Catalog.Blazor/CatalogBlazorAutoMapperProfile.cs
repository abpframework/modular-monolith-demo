using AutoMapper;
using Shopularity.Catalog.Services.Categories.Admin;
using Shopularity.Catalog.Services.Products.Admin;

namespace Shopularity.Catalog.Blazor;

public class CatalogBlazorAutoMapperProfile : Profile
{
    public CatalogBlazorAutoMapperProfile()
    {
        CreateMap<CategoryDto, CategoryUpdateDto>();

        CreateMap<ProductDto, ProductUpdateDto>();
    }
}