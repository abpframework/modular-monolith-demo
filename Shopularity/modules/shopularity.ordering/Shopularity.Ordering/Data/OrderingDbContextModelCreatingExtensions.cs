using Volo.Abp.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace Shopularity.Ordering.Data;

public static class OrderingDbContextModelCreatingExtensions
{
    public static void ConfigureOrdering(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(OrderingDbProperties.DbTablePrefix + "Questions", OrderingDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */
        if (builder.IsHostDatabase())
        {
            builder.Entity<Order>(b =>
            {
                b.ToTable(OrderingDbProperties.DbTablePrefix + "Orders", OrderingDbProperties.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.UserId).HasColumnName(nameof(Order.UserId)).IsRequired();
                b.Property(x => x.State).HasColumnName(nameof(Order.State));
                b.Property(x => x.TotalPrice).HasColumnName(nameof(Order.TotalPrice));
                b.Property(x => x.ShippingAddress).HasColumnName(nameof(Order.ShippingAddress)).IsRequired().HasMaxLength(OrderConsts.ShippingAddressMaxLength);
                b.Property(x => x.CargoNo).HasColumnName(nameof(Order.CargoNo)).HasMaxLength(OrderConsts.CargoNoMaxLength);
            });

        }
    }
}