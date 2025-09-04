using Volo.Abp.Application.Dtos;

namespace Shopularity.Catalog.Services.Products.Public;

public class GetProductsPublicInput : PagedAndSortedResultRequestDto
{
    public string? CategoryName { get; set; }
}