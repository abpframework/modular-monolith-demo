using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Shopularity.Payment.Data;

[ConnectionStringName(PaymentDbProperties.ConnectionStringName)]
public interface IPaymentDbContext : IEfCoreDbContext
{
    DbSet<Domain.Payments.Payment> Payments { get; set; }
}