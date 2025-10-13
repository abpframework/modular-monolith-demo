using Volo.Abp.Application.Dtos;

namespace Shopularity.Catalog.Services.Categories.Admin;

public class GetCategoriesInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }
}