using Volo.Abp.Application.Dtos;

namespace Shopularity.Catalog.Categories
{
    public class GetCategoriesInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }

        public string? Name { get; set; }

        public GetCategoriesInput()
        {

        }
    }
}