using System.Collections.Generic;
using System.Threading.Tasks;
using Shopularity.Catalog.Domain.Categories;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Shopularity.Catalog.Services.Categories.Public;

[RemoteService(IsEnabled = false)]
public class CategoriesPublicAppService : CatalogAppService, ICategoriesPublicAppService
{
    protected ICategoryRepository _categoryRepository;

    public CategoriesPublicAppService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<ListResultDto<CategoryPublicDto>> GetListAsync()
    {
        var items = await _categoryRepository.GetListAsync();

        return new ListResultDto<CategoryPublicDto>
        {
            Items = ObjectMapper.Map<List<Category>, List<CategoryPublicDto>>(items)
        };
    }
}