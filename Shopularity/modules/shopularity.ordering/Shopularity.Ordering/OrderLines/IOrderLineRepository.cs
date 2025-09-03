using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Shopularity.Ordering.OrderLines;

//TODO: Discard IOrderLineRepository
public interface IOrderLineRepository : IRepository<OrderLine, Guid>
{
    Task<List<OrderLine>> GetListByOrderIdAsync(
        Guid orderId,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);

    Task<List<OrderLine>> GetListAsync(
        string? filterText = null,
        string? productId = null,
        string? name = null,
        int? amountMin = null,
        int? amountMax = null,
        double? totalPriceMin = null,
        double? totalPriceMax = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        string? filterText = null,
        string? productId = null,
        string? name = null,
        int? amountMin = null,
        int? amountMax = null,
        double? totalPriceMin = null,
        double? totalPriceMax = null,
        CancellationToken cancellationToken = default);
}