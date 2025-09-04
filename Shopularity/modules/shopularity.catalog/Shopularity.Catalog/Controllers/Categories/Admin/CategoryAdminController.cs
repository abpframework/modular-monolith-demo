using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Catalog.Services.Categories.Admin;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Catalog.Controllers.Categories.Admin;

[RemoteService(Name = "Catalog")]
[Area("catalog")]
[ControllerName("Category")]
[Route("api/catalog/categories")]
public class CategoryAdminController : AbpController, ICategoriesAdminAppService
{
    protected ICategoriesAdminAppService CategoriesAdminAppService;

    public CategoryAdminController(ICategoriesAdminAppService categoriesAdminAppService)
    {
        CategoriesAdminAppService = categoriesAdminAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<CategoryDto>> GetListAsync(GetCategoriesInput input)
    {
        return CategoriesAdminAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<CategoryDto> GetAsync(Guid id)
    {
        return CategoriesAdminAppService.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<CategoryDto> CreateAsync(CategoryCreateDto input)
    {
        return CategoriesAdminAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<CategoryDto> UpdateAsync(Guid id, CategoryUpdateDto input)
    {
        return CategoriesAdminAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return CategoriesAdminAppService.DeleteAsync(id);
    }
}