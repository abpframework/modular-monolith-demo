using System;
using System.Threading.Tasks;
using Shopularity.Payment.Payments.Events;
using Volo.Abp.Domain.Repositories;
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
        private readonly PaymentFakeEventService _paymentFakeEventService;

        public PaymentManager(
            IPaymentRepository paymentRepository,
            IUnitOfWorkAccessor unitOfWorkAccessor,
            IDistributedEventBus eventBus,
            PaymentFakeEventService  paymentFakeEventService)
        {
            _paymentRepository = paymentRepository;
            _unitOfWorkAccessor = unitOfWorkAccessor;
            _eventBus = eventBus;
            _paymentFakeEventService = paymentFakeEventService;
        }

        public virtual async Task<Payment> CreateAsync(Guid orderId)
        {
            var payment = new Payment(GuidGenerator.Create(), orderId.ToString())
            {
                State = PaymentState.Waiting
            };
            
            payment = await _paymentRepository.InsertAsync(payment);
            await _unitOfWorkAccessor.UnitOfWork!.SaveChangesAsync();

            await _eventBus.PublishAsync(new PaymentCreatedEto
            {
                OrderId = orderId
            });
            
            await _paymentFakeEventService.CompletePaymentAsync(payment.Id);
            
            return payment;
        }

        public async Task CancelAsync(Guid orderId)
        {
            var payment = await _paymentRepository.FirstOrDefaultAsync(x=> x.OrderId == orderId.ToString());

            if (payment == null)
            {
                return;
            }

            if (payment.State is PaymentState.Cancelled or PaymentState.Refunded)
            {
                return;
            }
            
            if (payment.State is PaymentState.Waiting or PaymentState.Failed)
            {
                payment.State = PaymentState.Cancelled;
            }
            else if (payment.State is PaymentState.Completed)
            {
                // refund process not implemented. Ideally we would have a state called "RefundRequested" etc.
                payment.State = PaymentState.Refunded;
            }
            
            await _paymentRepository.UpdateAsync(payment);
        }
    }
}