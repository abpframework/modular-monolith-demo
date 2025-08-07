using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace Shopularity.Payment.Payments
{
    public class PaymentManager : DomainService
    {
        protected IPaymentRepository _paymentRepository;

        public PaymentManager(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public virtual async Task<Payment> CreateAsync(string orderId)
        {
            Check.NotNullOrWhiteSpace(orderId, nameof(orderId));

            var payment = new Payment(GuidGenerator.Create(),orderId);

            return await _paymentRepository.InsertAsync(payment);
        }

        public virtual async Task<Payment> UpdateAsync(Guid id, [CanBeNull] string? concurrencyStamp = null)
        {
            var payment = await _paymentRepository.GetAsync(id);

            payment.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _paymentRepository.UpdateAsync(payment);
        }
    }
}