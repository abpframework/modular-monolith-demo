using System;
using Volo.Abp.Application.Dtos;

namespace Shopularity.Catalog.Services.Products.Public;

public class GetProductsPublicInput : PagedAndSortedResultRequestDto
{
    public Guid? CategoryId { get; set; }
}