using System;
using System.Threading.Tasks;
using Shopularity.Payment.Payments.Events;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;

namespace Shopularity.Payment.Payments;

public class PaymentManager : DomainService
{
    protected IPaymentRepository _paymentRepository;
    private readonly IDistributedEventBus _eventBus;
    private readonly IBackgroundJobManager _backgroundJobManager;

    public PaymentManager(
        IPaymentRepository paymentRepository,
        IDistributedEventBus eventBus,
        IBackgroundJobManager backgroundJobManager)
    {
        _paymentRepository = paymentRepository;
        _eventBus = eventBus;
        _backgroundJobManager = backgroundJobManager;
    }

    public virtual async Task<Payment> CreateAsync(Guid orderId, double totalPrice)
    {
        var payment = new Payment(GuidGenerator.Create(), orderId)
        {
            State = PaymentState.Waiting,
            TotalPrice = totalPrice
        };

        payment = await _paymentRepository.InsertAsync(payment, autoSave: true);

        await _eventBus.PublishAsync(new PaymentCreatedEto
        {
            PaymentId = payment.Id,
            OrderId = orderId
        });

        await _backgroundJobManager.EnqueueAsync(new PaymentFakeEventJob.PaymentFakeEventJobArgs
            {
                PaymentId = payment.Id,
            },
            delay: TimeSpan.FromSeconds(60));

        return payment;
    }

    public async Task CancelAsync(Guid orderId)
    {
        var payment = await _paymentRepository.FirstOrDefaultAsync(x => x.OrderId == orderId);

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