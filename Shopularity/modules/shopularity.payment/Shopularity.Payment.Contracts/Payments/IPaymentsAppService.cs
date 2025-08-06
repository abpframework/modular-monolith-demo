using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Shopularity.Payment.Payments
{
    public interface IPaymentsAppService : IApplicationService
    {

        Task<PagedResultDto<PaymentDto>> GetListAsync(GetPaymentsInput input);

        Task<PaymentDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<PaymentDto> CreateAsync(PaymentCreateDto input);

        Task<PaymentDto> UpdateAsync(Guid id, PaymentUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(PaymentExcelDownloadDto input);

        Task<Shopularity.Payment.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}