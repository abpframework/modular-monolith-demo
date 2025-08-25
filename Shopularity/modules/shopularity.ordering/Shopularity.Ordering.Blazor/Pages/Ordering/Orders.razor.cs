using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Web;
using Blazorise;
using Blazorise.DataGrid;
using Volo.Abp.BlazoriseUI.Components;
using Microsoft.AspNetCore.Authorization;
using Shopularity.Ordering.Localization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Shopularity.Ordering.Orders;
using Shopularity.Ordering.Permissions;
using Shopularity.Ordering.OrderLines; 

namespace Shopularity.Ordering.Blazor.Pages.Ordering;

public partial class Orders
{
    protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
    protected PageToolbar Toolbar {get;} = new PageToolbar();
    protected bool ShowAdvancedFilters { get; set; }
    public DataGrid<OrderDto> DataGridRef { get; set; }
    private IReadOnlyList<OrderDto> OrderList { get; set; }
    private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
    private int CurrentPage { get; set; } = 1;
    private string CurrentSorting { get; set; } = string.Empty;
    private int TotalCount { get; set; }
    private bool CanEditOrder { get; set; }
    private bool CanSetShipment { get; set; }
    private OrderDto EditingOrder { get; set; }
    private SetShippingInfoInput Shipment { get; set; }
    private Validations EditingOrderValidations { get; set; } = new();
    private Validations ShipmentValidations { get; set; } = new();
    private Guid EditingOrderId { get; set; }
    private Guid ShippingOrderId { get; set; }
    private Modal EditOrderModal { get; set; } = new();
    private Modal ShippingModal { get; set; } = new();
    private GetOrdersInput Filter { get; set; }
    private DataGridEntityActionsColumn<OrderDto> EntityActionsColumn { get; set; } = new();
    private OrderDto? SelectedOrder;

    private bool CanListOrderLine { get; set; }
    private Dictionary<Guid, DataGrid<OrderLineDto>> OrderLineDataGrids { get; set; } = new();
    private int OrderLinePageSize { get; } = 5;
        
    public Orders()
    {
        LocalizationResource = typeof(OrderingResource);
            
        EditingOrder = new OrderDto();
        Shipment = new SetShippingInfoInput();
        Filter = new GetOrdersInput
        {
            MaxResultCount = PageSize,
            SkipCount = (CurrentPage - 1) * PageSize,
            Sorting = CurrentSorting
        };
        OrderList = new List<OrderDto>();
    }

    protected override async Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SetBreadcrumbItemsAsync();
            await SetToolbarItemsAsync();
            await InvokeAsync(StateHasChanged);
        }
    }  

    protected virtual ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Orders"]));
        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);

        return ValueTask.CompletedTask;
    }
        
    private void ToggleDetails(OrderDto order)
    {
        DataGridRef.ToggleDetailRow(order, true);
    }
            
    private bool DetailRowTriggerHandler(DetailRowTriggerEventArgs<OrderDto> detailRowTriggerEventArgs)
    {
        detailRowTriggerEventArgs.Toggleable = false;
        detailRowTriggerEventArgs.DetailRowTriggerType = DetailRowTriggerType.Manual;
        return true;
    }

    private async Task SetPermissionsAsync()
    {
        CanEditOrder = await AuthorizationService
            .IsGrantedAsync(OrderingPermissions.Orders.Edit);
        CanSetShipment = await AuthorizationService
            .IsGrantedAsync(OrderingPermissions.Orders.SetShippingInfo);
            
        CanListOrderLine = await AuthorizationService
            .IsGrantedAsync(OrderingPermissions.OrderLines.Default);   
    }

    private async Task GetOrdersAsync()
    {
        Filter.MaxResultCount = PageSize;
        Filter.SkipCount = (CurrentPage - 1) * PageSize;
        Filter.Sorting = CurrentSorting;

        var result = await OrdersAppService.GetListAsync(Filter);
        OrderList = result.Items;

        foreach (var orderDto in OrderList)
        {
            if (orderDto.ShippingAddress.Length > 50)
            {
                orderDto.ShippingAddress = orderDto.ShippingAddress[..47] + "...";
            }
        }
        
        TotalCount = (int)result.TotalCount;
    }

    protected virtual async Task SearchAsync()
    {
        CurrentPage = 1;
        await GetOrdersAsync();
        await InvokeAsync(StateHasChanged);
    }

    private async Task DownloadAsExcelAsync()
    {
        var token = (await OrdersAppService.GetDownloadTokenAsync()).Token;
        var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Ordering") ?? await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
        var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
        if(!culture.IsNullOrEmpty())
        {
            culture = "&culture=" + culture;
        }
        await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
        NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/ordering/orders/as-excel-file?DownloadToken={token}&FilterText={HttpUtility.UrlEncode(Filter.FilterText)}{culture}&UserId={HttpUtility.UrlEncode(Filter.UserId.ToString())}&State={Filter.State}&TotalPriceMin={Filter.TotalPriceMin}&TotalPriceMax={Filter.TotalPriceMax}&ShippingAddress={HttpUtility.UrlEncode(Filter.ShippingAddress)}&CargoNo={HttpUtility.UrlEncode(Filter.CargoNo)}", forceLoad: true);
    }

    private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<OrderDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page;
        await GetOrdersAsync();
        await InvokeAsync(StateHasChanged);
    }

    private async Task OpenEditOrderModalAsync(OrderDto input)
    {
        var order = await OrdersAppService.GetAsync(input.Id);
            
        EditingOrderId = order.Id;
        EditingOrder = order;
            
        await EditingOrderValidations.ClearAll();
        await EditOrderModal.Show();
    }

    private async Task OpenShipmentModalAsync(OrderDto input)
    {
        var order = await OrdersAppService.GetAsync(input.Id);
            
        ShippingOrderId = order.Id;
        Shipment = ObjectMapper.Map<OrderDto, SetShippingInfoInput>(order);
            
        await ShipmentValidations.ClearAll();
        await ShippingModal.Show();
    }

    private async Task CloseEditOrderModalAsync()
    {
        await EditOrderModal.Hide();
    }

    private async Task CloseShipmentModalAsync()
    {
        await ShippingModal.Hide();
    }

    private async Task UpdateOrderAsync()
    {
        try
        {
            if (await EditingOrderValidations.ValidateAll() == false)
            {
                return;
            }

            await OrdersAppService.UpdateAsync(EditingOrderId, new OrderUpdateDto
            {
                ShippingAddress = EditingOrder.ShippingAddress,
                ConcurrencyStamp = EditingOrder.ConcurrencyStamp
            });
            await GetOrdersAsync();
            await EditOrderModal.Hide();                
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    private async Task SetShipmentAsync()
    {
        try
        {
            if (await ShipmentValidations.ValidateAll() == false)
            {
                return;
            }

            await OrdersAppService.SetShippingInfoAsync(ShippingOrderId, Shipment);
            await GetOrdersAsync();
            await ShippingModal.Hide();                
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
        
    protected virtual async Task OnStateChangedAsync(OrderState? state)
    {
        Filter.State = state;
        await SearchAsync();
    }
        
    protected virtual async Task OnTotalPriceMinChangedAsync(double? totalPriceMin)
    {
        Filter.TotalPriceMin = totalPriceMin;
        await SearchAsync();
    }
        
    protected virtual async Task OnTotalPriceMaxChangedAsync(double? totalPriceMax)
    {
        Filter.TotalPriceMax = totalPriceMax;
        await SearchAsync();
    }
        
    protected virtual async Task OnShippingAddressChangedAsync(string? shippingAddress)
    {
        Filter.ShippingAddress = shippingAddress;
        await SearchAsync();
    }
        
    protected virtual async Task OnCargoNoChangedAsync(string? cargoNo)
    {
        Filter.CargoNo = cargoNo;
        await SearchAsync();
    }

    private bool ShouldShowDetailRow()
    {
        return CanListOrderLine;
    }

    public string SelectedChildTab { get; set; } = "orderline-tab";

    private Task OnSelectedChildTabChanged(string name)
    {
        SelectedChildTab = name;

        return Task.CompletedTask;
    }

    private async Task OnOrderLineDataGridReadAsync(DataGridReadDataEventArgs<OrderLineDto> e, Guid orderId)
    {
        var sorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");

        var currentPage = e.Page;
        await SetOrderLinesAsync(orderId, currentPage, sorting: sorting);
        await InvokeAsync(StateHasChanged);
    }
        
    private async Task SetOrderLinesAsync(Guid orderId, int currentPage = 1, string? sorting = null)
    {
        var order = OrderList.FirstOrDefault(x => x.Id == orderId);
        if(order == null)
        {
            return;
        }

        var orderLines = await OrderLinesAppService.GetListByOrderIdAsync(new GetOrderLineListInput 
        {
            OrderId = orderId,
            MaxResultCount = OrderLinePageSize,
            SkipCount = (currentPage - 1) * OrderLinePageSize,
            Sorting = sorting
        });

        order.OrderLines = orderLines.Items.ToList();

        var orderLineDataGrid = OrderLineDataGrids[orderId];
            
        orderLineDataGrid.CurrentPage = currentPage;
        orderLineDataGrid.TotalItems = (int)orderLines.TotalCount;
    }
}