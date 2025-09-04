using System;
using Shopularity.Payment.Domain.Payments;
using Volo.Abp.Application.Dtos;

namespace Shopularity.Payment.Services.Payments;

public class GetPaymentsInput : PagedAndSortedResultRequestDto
{
    public Guid? OrderId { get; set; }
    public PaymentState? State { get; set; }

    public GetPaymentsInput()
    {

    }
}