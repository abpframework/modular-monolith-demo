using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Shopularity.Payment.Data;

namespace Shopularity.Payment.Payments
{
    public class EfCorePaymentRepository : EfCoreRepository<PaymentDbContext, Payment, Guid>, IPaymentRepository
    {
        public EfCorePaymentRepository(IDbContextProvider<PaymentDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<List<Payment>> GetListAsync(
            string? filterText = null,
            string? orderId = null,
            PaymentState? state = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, orderId, state);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? PaymentConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? orderId = null,
            PaymentState? state = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, orderId, state);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Payment> ApplyFilter(
            IQueryable<Payment> query,
            string? filterText = null,
            string? orderId = null,
            PaymentState? state = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.OrderId!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(orderId), e => e.OrderId.Contains(orderId))
                    .WhereIf(state.HasValue, e => e.State == state);
        }
    }
}