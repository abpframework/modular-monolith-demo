using System;
using Volo.Abp.Application.Dtos;

namespace Shopularity.Payment.Payments;

public class GetPaymentsInput : PagedAndSortedResultRequestDto
{
    public Guid? OrderId { get; set; }
    public PaymentState? State { get; set; }

    public GetPaymentsInput()
    {

    }
}