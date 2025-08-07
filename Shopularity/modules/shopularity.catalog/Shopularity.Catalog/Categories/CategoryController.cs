using Asp.Versioning;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace Shopularity.Catalog.Categories;

[RemoteService(Name = "Catalog")]
[Area("catalog")]
[ControllerName("Category")]
[Route("api/catalog/categories")]
public class CategoryController : AbpController, ICategoriesAppService
{
    protected ICategoriesAppService _categoriesAppService;

    public CategoryController(ICategoriesAppService categoriesAppService)
    {
        _categoriesAppService = categoriesAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<CategoryDto>> GetListAsync(GetCategoriesInput input)
    {
        return _categoriesAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<CategoryDto> GetAsync(Guid id)
    {
        return _categoriesAppService.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<CategoryDto> CreateAsync(CategoryCreateDto input)
    {
        return _categoriesAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<CategoryDto> UpdateAsync(Guid id, CategoryUpdateDto input)
    {
        return _categoriesAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _categoriesAppService.DeleteAsync(id);
    }
}