using Shopularity.Ordering.Orders;
using Volo.Abp.Application.Dtos;
using System;

namespace Shopularity.Ordering.Orders;

public class OrderExcelDownloadDto
{
    public string DownloadToken { get; set; } = null!;

    public OrderState? State { get; set; }
    public double? TotalPriceMin { get; set; }
    public double? TotalPriceMax { get; set; }
    public string? ShippingAddress { get; set; }
    public string? CargoNo { get; set; }
    public string? Username { get; set; }

    public OrderExcelDownloadDto()
    {

    }
}