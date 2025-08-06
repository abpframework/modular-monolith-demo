using System;
using Volo.Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;

using Volo.Abp;

namespace Shopularity.Payment.Payments
{
    public class Payment : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string OrderId { get; set; }

        public virtual PaymentState State { get; set; }

        protected Payment()
        {

        }

        public Payment(Guid id, string orderId, PaymentState state)
        {

            Id = id;
            Check.NotNull(orderId, nameof(orderId));
            OrderId = orderId;
            State = state;
        }

    }
}