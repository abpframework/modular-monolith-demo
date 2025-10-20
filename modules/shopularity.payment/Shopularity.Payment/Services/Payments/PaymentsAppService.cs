using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using MiniExcelLibs;
using Shopularity.Payment.Domain.Payments;
using Shopularity.Payment.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Content;

namespace Shopularity.Payment.Services.Payments;

[RemoteService(IsEnabled = false)]
[Authorize(PaymentPermissions.Payments.Default)]
public class PaymentsAppService : PaymentAppService, IPaymentsAppService
{
    protected IDistributedCache<PaymentDownloadTokenCacheItem, string> _downloadTokenCache;
    protected IPaymentRepository _paymentRepository;

    public PaymentsAppService(
        IPaymentRepository paymentRepository,
        IDistributedCache<PaymentDownloadTokenCacheItem, string> downloadTokenCache)
    {
        _downloadTokenCache = downloadTokenCache;
        _paymentRepository = paymentRepository;
    }

    public virtual async Task<PagedResultDto<PaymentDto>> GetListAsync(GetPaymentsInput input)
    {
        var totalCount = await _paymentRepository.GetCountAsync(input.OrderId, input.State);
        var items = await _paymentRepository.GetListAsync(input.OrderId, input.State, input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<PaymentDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<Domain.Payments.Payment>, List<PaymentDto>>(items)
        };
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(PaymentExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var items = await _paymentRepository.GetListAsync(input.OrderId, input.State);

        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Domain.Payments.Payment>, List<PaymentExcelDto>>(items));
        memoryStream.Seek(0, SeekOrigin.Begin);

        return new RemoteStreamContent(memoryStream, "Payments.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    public virtual async Task<Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");

        await _downloadTokenCache.SetAsync(
            token,
            new PaymentDownloadTokenCacheItem { Token = token },
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

        return new Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }
}