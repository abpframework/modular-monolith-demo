using System;
using System.Threading.Tasks;
using Shopularity.Ordering.Services.Orders.OrderLines;
using Shopularity.Ordering.Shared;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Shopularity.Ordering.Services.Orders.Admin;

public interface IOrdersAdminAppService : IApplicationService
{
    Task<PagedResultDto<OrderDto>> GetListAsync(GetOrdersInput input);

    Task<OrderDto> GetAsync(Guid id);

    Task<OrderDto> SetShipmentAddressInfoAsync(Guid id, OrderUpdateDto input);

    Task<OrderDto> SetShipmentCargoNoAsync(Guid id, SetShipmentCargoNoInput input);

    Task<IRemoteStreamContent> GetListAsExcelFileAsync(OrderExcelDownloadDto input);

    Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    
    Task<PagedResultDto<OrderLineDto>> GetOrderLineListAsync(GetOrderLineListInput input);
}