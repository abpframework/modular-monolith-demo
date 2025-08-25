using System;
using System.Threading.Tasks;
using Shopularity.Payment.Payments.Events;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Shopularity.Payment.Payments;

public class PaymentFakeEventJob : AsyncBackgroundJob<PaymentFakeEventJob.PaymentFakeEventJobArgs>, ITransientDependency
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IDistributedEventBus _eventBus;

    public PaymentFakeEventJob(
        IPaymentRepository paymentRepository,
        IDistributedEventBus eventBus)
    {
        _paymentRepository = paymentRepository;
        _eventBus = eventBus;
    }

    public override async Task ExecuteAsync(PaymentFakeEventJobArgs args)
    {
        var payment = await _paymentRepository.GetAsync(args.PaymentId);

        if (payment.State != PaymentState.Waiting)
        {
            return;
        }
            
        payment.State = PaymentState.Completed;
            
        await _paymentRepository.UpdateAsync(payment);

        await _eventBus.PublishAsync(new PaymentCompletedEto
        {
            PaymentId = args.PaymentId,
            OrderId = payment.OrderId
        });
    }

    public class PaymentFakeEventJobArgs
    {
        public Guid PaymentId { get; set; }
    }
}