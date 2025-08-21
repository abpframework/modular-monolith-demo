using System;
using System.Threading.Tasks;
using Shopularity.Payment.Payments.Events;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace Shopularity.Payment.Payments
{
    public class PaymentManager : DomainService
    {
        protected IPaymentRepository _paymentRepository;
        private readonly IUnitOfWorkAccessor _unitOfWorkAccessor;
        private readonly IDistributedEventBus _eventBus;

        public PaymentManager(
            IPaymentRepository paymentRepository,
            IUnitOfWorkAccessor unitOfWorkAccessor,
            IDistributedEventBus eventBus)
        {
            _paymentRepository = paymentRepository;
            _unitOfWorkAccessor = unitOfWorkAccessor;
            _eventBus = eventBus;
        }

        public virtual async Task<Payment> CreateAsync(Guid orderId)
        {
            var payment = new Payment(GuidGenerator.Create(), orderId.ToString())
            {
                State = PaymentState.Waiting
            };
            
            payment = await _paymentRepository.InsertAsync(payment);
            await _unitOfWorkAccessor.UnitOfWork!.SaveChangesAsync();
            
            _ = Task.Run(async () =>
            {
                await FakePaymentCompleteAsync(payment.Id);
            });

            return payment;
        }

        private async Task FakePaymentCompleteAsync(Guid paymentId)
        {
            await Task.Delay(1000 * 60);
            
            var payment = await _paymentRepository.GetAsync(paymentId);

            if (payment.State != PaymentState.Waiting)
            {
                return;
            }
            
            payment.State = PaymentState.Completed;
            
            await _paymentRepository.UpdateAsync(payment);

            await _eventBus.PublishAsync(new PaymentCompletedEto
            {
                PaymentId = paymentId,
                OrderId = payment.OrderId
            });
        }
    }
}