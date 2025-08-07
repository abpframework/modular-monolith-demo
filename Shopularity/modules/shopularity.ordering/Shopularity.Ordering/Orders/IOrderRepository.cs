using Shopularity.Ordering.Orders;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Shopularity.Ordering.Orders
{
    public interface IOrderRepository : IRepository<Order, Guid>
    {
        Task<List<Order>> GetListAsync(
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
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? userId = null,
            OrderState? state = null,
            double? totalPriceMin = null,
            double? totalPriceMax = null,
            string? shippingAddress = null,
            string? cargoNo = null,
            CancellationToken cancellationToken = default);
    }
}