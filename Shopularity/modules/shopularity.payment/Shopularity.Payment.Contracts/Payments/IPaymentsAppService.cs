using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Shopularity.Payment.Payments;

public interface IPaymentsAppService : IApplicationService
{

    Task<PagedResultDto<PaymentDto>> GetListAsync(GetPaymentsInput input);

    Task<PaymentDto> GetAsync(Guid id);

    Task<IRemoteStreamContent> GetListAsExcelFileAsync(PaymentExcelDownloadDto input);

    Task<Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

}