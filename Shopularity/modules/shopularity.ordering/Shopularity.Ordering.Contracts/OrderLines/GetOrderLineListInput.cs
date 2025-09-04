using Volo.Abp.Application.Dtos;
using System;

namespace Shopularity.Ordering.OrderLines;

public class GetOrderLineListInput : PagedResultRequestDto
{
    public Guid OrderId { get; set; }
}