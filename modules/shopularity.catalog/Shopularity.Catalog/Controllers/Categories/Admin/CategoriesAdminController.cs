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
public class CategoriesAdminController : AbpController, ICategoriesAdminAppService
{
    private readonly ICategoriesAdminAppService _categoriesAdminAppService;

    public CategoriesAdminController(ICategoriesAdminAppService categoriesAdminAppService)
    {
        _categoriesAdminAppService = categoriesAdminAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<CategoryDto>> GetListAsync(GetCategoriesInput input)
    {
        return _categoriesAdminAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<CategoryDto> GetAsync(Guid id)
    {
        return _categoriesAdminAppService.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<CategoryDto> CreateAsync(CategoryCreateDto input)
    {
        return _categoriesAdminAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<CategoryDto> UpdateAsync(Guid id, CategoryUpdateDto input)
    {
        return _categoriesAdminAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _categoriesAdminAppService.DeleteAsync(id);
    }
}