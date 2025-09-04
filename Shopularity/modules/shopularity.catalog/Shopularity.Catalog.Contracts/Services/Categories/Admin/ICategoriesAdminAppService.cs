using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Shopularity.Catalog.Services.Categories.Admin;

public interface ICategoriesAdminAppService : IApplicationService
{
    Task<PagedResultDto<CategoryDto>> GetListAsync(GetCategoriesInput input);

    Task<CategoryDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<CategoryDto> CreateAsync(CategoryCreateDto input);

    Task<CategoryDto> UpdateAsync(Guid id, CategoryUpdateDto input);
}