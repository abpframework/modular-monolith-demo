using Volo.Abp.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace Shopularity.Payment.Data;

public static class PaymentDbContextModelCreatingExtensions
{
    public static void ConfigurePayment(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<Domain.Payments.Payment>(b =>
        {
            b.ToTable(PaymentDbProperties.DbTablePrefix + "Payments", PaymentDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.OrderId).HasColumnName(nameof(Domain.Payments.Payment.OrderId)).IsRequired();
            b.Property(x => x.TotalPrice).HasColumnName(nameof(Domain.Payments.Payment.TotalPrice));
            b.Property(x => x.State).HasColumnName(nameof(Domain.Payments.Payment.State));
        });
    }
}