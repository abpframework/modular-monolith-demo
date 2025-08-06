using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Shopularity.Basket.Data;

[ConnectionStringName(BasketDbProperties.ConnectionStringName)]
public interface IBasketDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
