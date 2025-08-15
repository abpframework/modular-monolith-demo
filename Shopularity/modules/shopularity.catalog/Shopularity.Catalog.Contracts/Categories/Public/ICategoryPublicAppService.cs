using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Shopularity.Catalog.Categories.Public;

public interface ICategoriesPublicAppService : IApplicationService
{
    Task<ListResultDto<CategoryPublicDto>> GetListAsync();
}