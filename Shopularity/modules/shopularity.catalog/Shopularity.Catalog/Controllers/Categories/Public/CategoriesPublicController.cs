using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Catalog.Services.Categories.Public;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Catalog.Controllers.Categories.Public;

[RemoteService(Name = "Catalog")]
[Area("catalog")]
[ControllerName("CategoryPublic")]
[Route("api/catalog/public/categories")]
public class CategoriesPublicController : AbpController, ICategoriesPublicAppService
{
    private readonly ICategoriesPublicAppService _categoriesAppService;

    public CategoriesPublicController(ICategoriesPublicAppService categoriesAppService)
    {
        _categoriesAppService = categoriesAppService;
    }

    [HttpGet]
    public virtual Task<ListResultDto<CategoryPublicDto>> GetListAsync()
    {
        return _categoriesAppService.GetListAsync();
    }
}