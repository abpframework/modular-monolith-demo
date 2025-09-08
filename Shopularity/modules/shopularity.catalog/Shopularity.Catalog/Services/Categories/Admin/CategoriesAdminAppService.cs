using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Shopularity.Catalog.Domain.Categories;
using Shopularity.Catalog.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Shopularity.Catalog.Services.Categories.Admin;

[RemoteService(IsEnabled = false)]
[Authorize(CatalogPermissions.Categories.Default)]
public class CategoriesAdminAppService : CatalogAppService, ICategoriesAdminAppService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly CategoryManager _categoryManager;

    public CategoriesAdminAppService(ICategoryRepository categoryRepository, CategoryManager categoryManager)
    {
        _categoryRepository = categoryRepository;
        _categoryManager = categoryManager;
    }

    public virtual async Task<PagedResultDto<CategoryDto>> GetListAsync(GetCategoriesInput input)
    {
        var totalCount = await _categoryRepository.GetCountAsync(input.FilterText);
        var items = await _categoryRepository.GetListAsync(input.FilterText, input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<CategoryDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<Category>, List<CategoryDto>>(items)
        };
    }

    public virtual async Task<CategoryDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Category, CategoryDto>(await _categoryRepository.GetAsync(id));
    }

    [Authorize(CatalogPermissions.Categories.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _categoryRepository.DeleteAsync(id);
    }

    [Authorize(CatalogPermissions.Categories.Create)]
    public virtual async Task<CategoryDto> CreateAsync(CategoryCreateDto input)
    {
        var category = await _categoryManager.CreateAsync(
            input.Name
        );

        return ObjectMapper.Map<Category, CategoryDto>(category);
    }

    [Authorize(CatalogPermissions.Categories.Edit)]
    public virtual async Task<CategoryDto> UpdateAsync(Guid id, CategoryUpdateDto input)
    {
        var category = await _categoryManager.UpdateAsync(
            id,
            input.Name, input.ConcurrencyStamp
        );

        return ObjectMapper.Map<Category, CategoryDto>(category);
    }
}