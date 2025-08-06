using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Shopularity.Payment.Permissions;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace Shopularity.Payment.Payments
{
    [RemoteService(IsEnabled = false)]
    [Authorize(PaymentPermissions.Payments.Default)]
    public class PaymentsAppService : PaymentAppService, IPaymentsAppService
    {
        protected IDistributedCache<PaymentDownloadTokenCacheItem, string> _downloadTokenCache;
        protected IPaymentRepository _paymentRepository;
        protected PaymentManager _paymentManager;

        public PaymentsAppService(IPaymentRepository paymentRepository, PaymentManager paymentManager, IDistributedCache<PaymentDownloadTokenCacheItem, string> downloadTokenCache)
        {
            _downloadTokenCache = downloadTokenCache;
            _paymentRepository = paymentRepository;
            _paymentManager = paymentManager;

        }

        public virtual async Task<PagedResultDto<PaymentDto>> GetListAsync(GetPaymentsInput input)
        {
            var totalCount = await _paymentRepository.GetCountAsync(input.FilterText, input.OrderId, input.State);
            var items = await _paymentRepository.GetListAsync(input.FilterText, input.OrderId, input.State, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<PaymentDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Payment>, List<PaymentDto>>(items)
            };
        }

        public virtual async Task<PaymentDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Payment, PaymentDto>(await _paymentRepository.GetAsync(id));
        }

        [Authorize(PaymentPermissions.Payments.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _paymentRepository.DeleteAsync(id);
        }

        [Authorize(PaymentPermissions.Payments.Create)]
        public virtual async Task<PaymentDto> CreateAsync(PaymentCreateDto input)
        {

            var payment = await _paymentManager.CreateAsync(
            input.OrderId, input.State
            );

            return ObjectMapper.Map<Payment, PaymentDto>(payment);
        }

        [Authorize(PaymentPermissions.Payments.Edit)]
        public virtual async Task<PaymentDto> UpdateAsync(Guid id, PaymentUpdateDto input)
        {

            var payment = await _paymentManager.UpdateAsync(
            id
            , input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Payment, PaymentDto>(payment);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(PaymentExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _paymentRepository.GetListAsync(input.FilterText, input.OrderId, input.State);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Payment>, List<PaymentExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Payments.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public virtual async Task<Shopularity.Payment.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new PaymentDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new Shopularity.Payment.Shared.DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}