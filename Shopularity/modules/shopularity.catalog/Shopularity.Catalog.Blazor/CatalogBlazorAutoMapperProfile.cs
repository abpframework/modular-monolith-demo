using Shopularity.Catalog.Products;
using Shopularity.Catalog.Categories;
using AutoMapper;

namespace Shopularity.Catalog.Blazor;

public class CatalogBlazorAutoMapperProfile : Profile
{
    public CatalogBlazorAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<CategoryDto, CategoryUpdateDto>();

        CreateMap<ProductDto, ProductUpdateDto>();
    }
}