using System;
using Volo.Abp.Application.Dtos;

namespace Shopularity.Ordering.Services.Orders.OrderLines;

public class GetOrderLineListInput : PagedResultRequestDto
{
    public Guid OrderId { get; set; }
}