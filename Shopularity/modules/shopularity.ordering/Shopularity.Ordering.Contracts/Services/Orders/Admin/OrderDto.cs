using System;
using System.Collections.Generic;
using Shopularity.Ordering.Domain.Orders;
using Shopularity.Ordering.Services.Orders.OrderLines;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Shopularity.Ordering.Services.Orders.Admin;

public class OrderDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = null!;
    public OrderState State { get; set; }
    public double TotalPrice { get; set; }
    public string ShippingAddress { get; set; } = null!;
    public string? CargoNo { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

    public List<OrderLineDto> OrderLines { get; set; } = new();
}