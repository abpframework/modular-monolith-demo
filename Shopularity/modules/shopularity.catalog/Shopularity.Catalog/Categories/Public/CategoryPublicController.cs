using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Catalog.Categories.Public;

[RemoteService(Name = "Catalog")]
[Area("catalog")]
[ControllerName("CategoryPublic")]
[Route("api/catalog/public/categories")]
public class CategoryPublicController : AbpController, ICategoriesPublicAppService
{
    protected ICategoriesPublicAppService _categoriesAppService;

    public CategoryPublicController(ICategoriesPublicAppService categoriesAppService)
    {
        _categoriesAppService = categoriesAppService;
    }

    [HttpGet]
    public virtual Task<ListResultDto<CategoryPublicDto>> GetListAsync()
    {
        return _categoriesAppService.GetListAsync();
    }
}