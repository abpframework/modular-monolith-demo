using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Shopularity.Payment.Payments
{
    public interface IPaymentRepository : IRepository<Payment, Guid>
    {
        Task<List<Payment>> GetListAsync(
            string? filterText = null,
            string? orderId = null,
            PaymentState? state = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? orderId = null,
            PaymentState? state = null,
            CancellationToken cancellationToken = default);
    }
}