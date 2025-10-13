using Microsoft.EntityFrameworkCore;
using Shopularity.Ordering.Domain.Orders;
using Shopularity.Ordering.Domain.Orders.OrderLines;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Shopularity.Ordering.Data;

[ConnectionStringName(OrderingDbProperties.ConnectionStringName)]
public interface IOrderingDbContext : IEfCoreDbContext
{
    DbSet<Order> Orders { get; set; }
    
    DbSet<OrderLine> OrderLines { get; set; }
}