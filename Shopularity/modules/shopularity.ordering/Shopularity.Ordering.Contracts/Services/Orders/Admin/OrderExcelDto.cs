using System;
using Shopularity.Ordering.Domain.Orders;

namespace Shopularity.Ordering.Services.Orders.Admin;

public class OrderExcelDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = null!;
    public OrderState State { get; set; }
    public double TotalPrice { get; set; }
    public string ShippingAddress { get; set; } = null!;
    public string? CargoNo { get; set; }
}