using Shopularity.Ordering.Orders;
using Volo.Abp.Application.Dtos;
using System;

namespace Shopularity.Ordering.Orders
{
    public class GetOrdersInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }

        public string? UserId { get; set; }
        public OrderState? State { get; set; }
        public double? TotalPriceMin { get; set; }
        public double? TotalPriceMax { get; set; }
        public string? ShippingAddress { get; set; }
        public string? CargoNo { get; set; }

        public GetOrdersInput()
        {

        }
    }
}