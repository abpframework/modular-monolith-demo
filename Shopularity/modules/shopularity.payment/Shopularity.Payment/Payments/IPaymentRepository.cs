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
            Guid? orderId = null,
            PaymentState? state = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            Guid? orderId = null,
            PaymentState? state = null,
            CancellationToken cancellationToken = default);
    }
}