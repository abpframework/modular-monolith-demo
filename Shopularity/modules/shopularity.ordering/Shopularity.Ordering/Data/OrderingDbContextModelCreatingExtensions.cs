using Volo.Abp.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore;
using Shopularity.Ordering.OrderLines;
using Shopularity.Ordering.Orders;
using Volo.Abp;

namespace Shopularity.Ordering.Data;

public static class OrderingDbContextModelCreatingExtensions
{
    public static void ConfigureOrdering(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

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
                b.HasMany(x => x.OrderLines).WithOne().HasForeignKey(x => x.OrderId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<OrderLine>(b =>
            {
                b.ToTable(OrderingDbProperties.DbTablePrefix + "OrderLines", OrderingDbProperties.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.ProductId).HasColumnName(nameof(OrderLine.ProductId)).IsRequired();
                b.Property(x => x.Name).HasColumnName(nameof(OrderLine.Name)).HasMaxLength(OrderLineConsts.NameMaxLength);
                b.Property(x => x.Price).HasColumnName(nameof(OrderLine.Price));
                b.Property(x => x.Amount).HasColumnName(nameof(OrderLine.Amount));
                b.Property(x => x.TotalPrice).HasColumnName(nameof(OrderLine.TotalPrice));
                b.HasOne<Order>().WithMany(x => x.OrderLines).HasForeignKey(x => x.OrderId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}