using Shopularity.Ordering.Orders;
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

namespace Shopularity.Ordering.Orders
{
    public class EfCoreOrderRepository : EfCoreRepository<OrderingDbContext, Order, Guid>, IOrderRepository
    {
        public EfCoreOrderRepository(IDbContextProvider<OrderingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<List<Order>> GetListAsync(
            string? filterText = null,
            string? userId = null,
            OrderState? state = null,
            double? totalPriceMin = null,
            double? totalPriceMax = null,
            string? shippingAddress = null,
            string? cargoNo = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await WithDetailsAsync(x=> x.OrderLines)), filterText, userId, state, totalPriceMin, totalPriceMax, shippingAddress, cargoNo);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? OrderConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? userId = null,
            OrderState? state = null,
            double? totalPriceMin = null,
            double? totalPriceMax = null,
            string? shippingAddress = null,
            string? cargoNo = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, userId, state, totalPriceMin, totalPriceMax, shippingAddress, cargoNo);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Order> ApplyFilter(
            IQueryable<Order> query,
            string? filterText = null,
            string? userId = null,
            OrderState? state = null,
            double? totalPriceMin = null,
            double? totalPriceMax = null,
            string? shippingAddress = null,
            string? cargoNo = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.UserId!.Contains(filterText!) || e.ShippingAddress!.Contains(filterText!) || e.CargoNo!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(userId), e => e.UserId.Contains(userId))
                    .WhereIf(state.HasValue, e => e.State == state)
                    .WhereIf(totalPriceMin.HasValue, e => e.TotalPrice >= totalPriceMin!.Value)
                    .WhereIf(totalPriceMax.HasValue, e => e.TotalPrice <= totalPriceMax!.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(shippingAddress), e => e.ShippingAddress.Contains(shippingAddress))
                    .WhereIf(!string.IsNullOrWhiteSpace(cargoNo), e => e.CargoNo.Contains(cargoNo));
        }
    }
}