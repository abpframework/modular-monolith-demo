using Asp.Versioning;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace Shopularity.Payment.Payments;

[RemoteService(Name = "Payment")]
[Area("payment")]
[ControllerName("Payment")]
[Route("api/payment/payments")]
public class PaymentController : AbpController, IPaymentsAppService
{
    protected IPaymentsAppService _paymentsAppService;

    public PaymentController(IPaymentsAppService paymentsAppService)
    {
        _paymentsAppService = paymentsAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<PaymentDto>> GetListAsync(GetPaymentsInput input)
    {
        return _paymentsAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<PaymentDto> GetAsync(Guid id)
    {
        return _paymentsAppService.GetAsync(id);
    }

    [HttpGet]
    [Route("as-excel-file")]
    public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(PaymentExcelDownloadDto input)
    {
        return _paymentsAppService.GetListAsExcelFileAsync(input);
    }

    [HttpGet]
    [Route("download-token")]
    public virtual Task<Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        return _paymentsAppService.GetDownloadTokenAsync();
    }

}