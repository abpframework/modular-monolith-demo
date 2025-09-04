using System;
using System.Threading.Tasks;
using Shopularity.Ordering.OrderLines;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Shopularity.Ordering.Shared;

namespace Shopularity.Ordering.Orders;

public interface IOrdersAppService : IApplicationService
{
    Task<PagedResultDto<OrderDto>> GetListAsync(GetOrdersInput input);

    Task<OrderDto> GetAsync(Guid id);

    Task<OrderDto> UpdateAsync(Guid id, OrderUpdateDto input);

    Task<OrderDto> SetShippingInfoAsync(Guid id, SetShippingInfoInput input);

    Task<IRemoteStreamContent> GetListAsExcelFileAsync(OrderExcelDownloadDto input);

    Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    
    Task<PagedResultDto<OrderLineDto>> GetOrderLineListAsync(GetOrderLineListInput input);
}