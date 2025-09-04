using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Payment.Services.Payments;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace Shopularity.Payment.Controllers.Payments;

[RemoteService(Name = "Payment")]
[Area("payment")]
[ControllerName("Payment")]
[Route("api/payment/payments")]
public class PaymentsController : AbpController, IPaymentsAppService
{
    private readonly IPaymentsAppService _paymentsAppService;

    public PaymentsController(IPaymentsAppService paymentsAppService)
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