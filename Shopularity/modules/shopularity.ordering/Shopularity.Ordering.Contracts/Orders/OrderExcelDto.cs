using Shopularity.Ordering.Orders;
using System;

namespace Shopularity.Ordering.Orders
{
    public class OrderExcelDto
    {
        public string UserId { get; set; } = null!;
        public OrderState State { get; set; }
        public double TotalPrice { get; set; }
        public string ShippingAddress { get; set; } = null!;
        public string? CargoNo { get; set; }
    }
}