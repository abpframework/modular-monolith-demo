namespace Shopularity.Ordering.Orders.Events;

public class OrderItemDto
{
    public virtual string ItemId { get; set; }

    public virtual string Name { get; set; }

    public virtual double Price { get; set; }

    public virtual int Amount { get; set; }
}