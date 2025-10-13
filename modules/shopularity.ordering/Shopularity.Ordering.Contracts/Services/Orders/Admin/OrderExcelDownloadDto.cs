using Shopularity.Ordering.Domain.Orders;

namespace Shopularity.Ordering.Services.Orders.Admin;

public class OrderExcelDownloadDto
{
    public string DownloadToken { get; set; } = null!;

    public OrderState? State { get; set; }
    
    public double? TotalPriceMin { get; set; }
    
    public double? TotalPriceMax { get; set; }
    
    public string? ShippingAddress { get; set; }
    
    public string? CargoNo { get; set; }
    
    public string? Username { get; set; }
}