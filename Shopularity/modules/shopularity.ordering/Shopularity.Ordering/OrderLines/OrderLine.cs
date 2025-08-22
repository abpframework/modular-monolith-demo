using System;
using Volo.Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;

using Volo.Abp;

namespace Shopularity.Ordering.OrderLines;

public class OrderLine : FullAuditedEntity<Guid>
{
    public virtual Guid OrderId { get; set; }

    [NotNull]
    public virtual string ProductId { get; set; }

    [CanBeNull]
    public virtual string? Name { get; set; }

    public virtual double Price { get; set; }

    public virtual int Amount { get; set; }

    public virtual double TotalPrice { get; set; }

    protected OrderLine()
    {

    }

    public OrderLine(Guid id, Guid orderId, string productId, double price, int amount, double totalPrice, string? name = null)
    {
        Id = id;
        Check.NotNull(productId, nameof(productId));
        Check.Length(name, nameof(name), OrderLineConsts.NameMaxLength, 0);
        OrderId = orderId;
        ProductId = productId;
        Price = price;
        Amount = amount;
        TotalPrice = totalPrice;
        Name = name;
    }
}