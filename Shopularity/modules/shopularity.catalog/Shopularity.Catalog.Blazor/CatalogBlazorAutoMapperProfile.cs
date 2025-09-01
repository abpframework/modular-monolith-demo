using Shopularity.Catalog.Categories;
using AutoMapper;
using Shopularity.Catalog.Products.Admin;

namespace Shopularity.Catalog.Blazor;

public class CatalogBlazorAutoMapperProfile : Profile
{
    public CatalogBlazorAutoMapperProfile()
    {
        CreateMap<CategoryDto, CategoryUpdateDto>();

        CreateMap<ProductDto, ProductUpdateDto>();
    }
}