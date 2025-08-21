using System;
using System.Threading.Tasks;
using Shopularity.Payment.Payments.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Shopularity.Payment.Payments;

public class PaymentFakeEventService: ISingletonDependency
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IDistributedEventBus _eventBus;

    public PaymentFakeEventService(IPaymentRepository paymentRepository, IDistributedEventBus eventBus)
    {
        _paymentRepository = paymentRepository;
        _eventBus = eventBus;
    }
    
    public async Task CompletePaymentAsync(Guid paymentId, int delaySeconds = 60)
    {
        _ = Task.Run(async () =>
        {
            await Task.Delay(1000 * delaySeconds);
            
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
        });
    }
}