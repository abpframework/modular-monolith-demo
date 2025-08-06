using Volo.Abp.Application.Dtos;

namespace Shopularity.Payment.Payments
{
    public class GetPaymentsInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }

        public string? OrderId { get; set; }
        public PaymentState? State { get; set; }

        public GetPaymentsInput()
        {

        }
    }
}