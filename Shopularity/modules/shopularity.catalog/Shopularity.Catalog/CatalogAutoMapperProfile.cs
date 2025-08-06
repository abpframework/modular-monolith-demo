using System;
using Shopularity.Catalog.Shared;
using Shopularity.Catalog.Categories;
using Volo.Abp.AutoMapper;
using AutoMapper;

namespace Shopularity.Catalog;

public class CatalogAutoMapperProfile : Profile
{
    public CatalogAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Category, CategoryDto>();
    }
}