using Volo.Abp.Application.Dtos;

namespace Shopularity.Ordering.OrderLines;

public class GetOrderLinesInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? ProductId { get; set; }
    public string? Name { get; set; }
    public int? AmountMin { get; set; }
    public int? AmountMax { get; set; }
    public double? TotalPriceMin { get; set; }
    public double? TotalPriceMax { get; set; }

    public GetOrderLinesInput()
    {

    }
}