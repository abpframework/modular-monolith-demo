using Volo.Abp.Application.Dtos;
using System;

namespace Shopularity.Ordering.OrderLines
{
    public class GetOrderLineListInput : PagedAndSortedResultRequestDto
    {
        public Guid OrderId { get; set; }
    }
}