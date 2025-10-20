namespace Shopularity.Ordering.Domain.Orders;

public enum OrderState
{
    /// <summary>
    /// The order has been created but not yet processed.
    /// </summary>
    New,

    /// <summary>
    /// The order is waiting for the customer to complete the payment.
    /// </summary>
    WaitingForPayment,

    /// <summary>
    /// The payment for the order has been received successfully.
    /// </summary>
    Paid,

    /// <summary>
    /// The order is being prepared before shipment.
    /// </summary>
    Processing,

    /// <summary>
    /// The order has been shipped to the customer.
    /// </summary>
    Shipped,

    /// <summary>
    /// The order has been delivered and completed successfully.
    /// </summary>
    Completed,

    /// <summary>
    /// The order has been canceled before completion.
    /// </summary>
    Cancelled
}
