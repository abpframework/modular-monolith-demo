using Shopularity.Ordering.Domain.Orders;
using Volo.Abp.Application.Dtos;

namespace Shopularity.Ordering.Services.Orders.Admin;

public class GetOrdersInput : PagedAndSortedResultRequestDto
{
    public string? Username { get; set; }
    
    public OrderState? State { get; set; }
    
    public double? TotalPriceMin { get; set; }
    
    public double? TotalPriceMax { get; set; }
    
    public string? ShippingAddress { get; set; }
    
    public string? CargoNo { get; set; }
}