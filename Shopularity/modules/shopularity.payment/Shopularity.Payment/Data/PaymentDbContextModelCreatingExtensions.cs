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

        if (builder.IsHostDatabase())
        {
            builder.Entity<Payments.Payment>(b =>
            {
                b.ToTable(PaymentDbProperties.DbTablePrefix + "Payments", PaymentDbProperties.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.OrderId).HasColumnName(nameof(Payments.Payment.OrderId)).IsRequired();
                b.Property(x => x.TotalPrice).HasColumnName(nameof(Payments.Payment.TotalPrice));
                b.Property(x => x.State).HasColumnName(nameof(Payments.Payment.State));
            });

        }
    }
}