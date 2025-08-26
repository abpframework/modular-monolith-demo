using Shopularity.Ordering.Orders;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Shopularity.Ordering.Orders;

public interface IOrderRepository : IRepository<Order, Guid>
{
    Task<List<Order>> GetListAsync(
        Guid? userId = null,
        OrderState? state = null,
        double? totalPriceMin = null,
        double? totalPriceMax = null,
        string? shippingAddress = null,
        string? cargoNo = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        Guid? userId = null,
        OrderState? state = null,
        double? totalPriceMin = null,
        double? totalPriceMax = null,
        string? shippingAddress = null,
        string? cargoNo = null,
        CancellationToken cancellationToken = default);
}