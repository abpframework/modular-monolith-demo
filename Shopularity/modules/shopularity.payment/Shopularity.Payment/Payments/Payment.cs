using System;
using Volo.Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;

using Volo.Abp;

namespace Shopularity.Payment.Payments
{
    public class Payment : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual Guid OrderId { get; private set; }

        public virtual PaymentState State { get; set; }

        protected Payment()
        {

        }

        public Payment(Guid id, Guid orderId)
        {
            Id = id;
            Check.NotNull(orderId, nameof(orderId));
            OrderId = orderId;
            State = PaymentState.Waiting;
        }
    }
}