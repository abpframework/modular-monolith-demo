using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Shopularity.Ordering.Data;

namespace Shopularity.Ordering.OrderLines;

public class EfCoreOrderLineRepository : EfCoreRepository<OrderingDbContext, OrderLine, Guid>, IOrderLineRepository
{
    public EfCoreOrderLineRepository(IDbContextProvider<OrderingDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<OrderLine>> GetListByOrderIdAsync(
        Guid orderId,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryableAsync()).Where(x => x.OrderId == orderId);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? OrderLineConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).Where(x => x.OrderId == orderId).CountAsync(cancellationToken);
    }

    public virtual async Task<List<OrderLine>> GetListAsync(
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
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, productId, name, amountMin, amountMax, totalPriceMin, totalPriceMax);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? OrderLineConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        string? filterText = null,
        string? productId = null,
        string? name = null,
        int? amountMin = null,
        int? amountMax = null,
        double? totalPriceMin = null,
        double? totalPriceMax = null,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetDbSetAsync()), filterText, productId, name, amountMin, amountMax, totalPriceMin, totalPriceMax);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<OrderLine> ApplyFilter(
        IQueryable<OrderLine> query,
        string? filterText = null,
        string? productId = null,
        string? name = null,
        int? amountMin = null,
        int? amountMax = null,
        double? totalPriceMin = null,
        double? totalPriceMax = null)
    {
        return query
            .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.ProductId!.Contains(filterText!) || e.Name!.Contains(filterText!))
            .WhereIf(!string.IsNullOrWhiteSpace(productId), e => e.ProductId.Contains(productId))
            .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.Name.Contains(name))
            .WhereIf(amountMin.HasValue, e => e.Amount >= amountMin!.Value)
            .WhereIf(amountMax.HasValue, e => e.Amount <= amountMax!.Value)
            .WhereIf(totalPriceMin.HasValue, e => e.TotalPrice >= totalPriceMin!.Value)
            .WhereIf(totalPriceMax.HasValue, e => e.TotalPrice <= totalPriceMax!.Value);
    }
}