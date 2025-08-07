using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Web;
using Blazorise;
using Blazorise.DataGrid;
using Volo.Abp.BlazoriseUI.Components;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Shopularity.Ordering.Orders;
using Shopularity.Ordering.Permissions;
using Shopularity.Ordering.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Volo.Abp;
using Volo.Abp.Content;


using Shopularity.Ordering.Orders;



namespace Shopularity.Ordering.Blazor.Pages.Ordering
{
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
        private bool CanCreateOrder { get; set; }
        private bool CanEditOrder { get; set; }
        private bool CanDeleteOrder { get; set; }
        private OrderCreateDto NewOrder { get; set; }
        private Validations NewOrderValidations { get; set; } = new();
        private OrderUpdateDto EditingOrder { get; set; }
        private Validations EditingOrderValidations { get; set; } = new();
        private Guid EditingOrderId { get; set; }
        private Modal CreateOrderModal { get; set; } = new();
        private Modal EditOrderModal { get; set; } = new();
        private GetOrdersInput Filter { get; set; }
        private DataGridEntityActionsColumn<OrderDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "order-create-tab";
        protected string SelectedEditTab = "order-edit-tab";
        private OrderDto? SelectedOrder;
        
        
        
        
        
        
        public Orders()
        {
            NewOrder = new OrderCreateDto();
            EditingOrder = new OrderUpdateDto();
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
            
            Toolbar.AddButton(L["NewOrder"], async () =>
            {
                await OpenCreateOrderModalAsync();
            }, IconName.Add, requiredPolicyName: OrderingPermissions.Orders.Create);

            return ValueTask.CompletedTask;
        }
        
        private void ToggleDetails(OrderDto order)
        {
            DataGridRef.ToggleDetailRow(order, true);
        }
        
        private bool RowSelectableHandler( RowSelectableEventArgs<OrderDto> rowSelectableEventArgs )
            => rowSelectableEventArgs.SelectReason is not DataGridSelectReason.RowClick && CanDeleteOrder;
            
        private bool DetailRowTriggerHandler(DetailRowTriggerEventArgs<OrderDto> detailRowTriggerEventArgs)
        {
            detailRowTriggerEventArgs.Toggleable = false;
            detailRowTriggerEventArgs.DetailRowTriggerType = DetailRowTriggerType.Manual;
            return true;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateOrder = await AuthorizationService
                .IsGrantedAsync(OrderingPermissions.Orders.Create);
            CanEditOrder = await AuthorizationService
                            .IsGrantedAsync(OrderingPermissions.Orders.Edit);
            CanDeleteOrder = await AuthorizationService
                            .IsGrantedAsync(OrderingPermissions.Orders.Delete);
                            
                            
        }

        private async Task GetOrdersAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await OrdersAppService.GetListAsync(Filter);
            OrderList = result.Items;
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
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/ordering/orders/as-excel-file?DownloadToken={token}&FilterText={HttpUtility.UrlEncode(Filter.FilterText)}{culture}&UserId={HttpUtility.UrlEncode(Filter.UserId)}&State={Filter.State}&TotalPriceMin={Filter.TotalPriceMin}&TotalPriceMax={Filter.TotalPriceMax}&ShippingAddress={HttpUtility.UrlEncode(Filter.ShippingAddress)}&CargoNo={HttpUtility.UrlEncode(Filter.CargoNo)}", forceLoad: true);
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

        private async Task OpenCreateOrderModalAsync()
        {
            NewOrder = new OrderCreateDto{
                
                
            };

            SelectedCreateTab = "order-create-tab";
            
            
            await NewOrderValidations.ClearAll();
            await CreateOrderModal.Show();
        }

        private async Task CloseCreateOrderModalAsync()
        {
            NewOrder = new OrderCreateDto{
                
                
            };
            await CreateOrderModal.Hide();
        }

        private async Task OpenEditOrderModalAsync(OrderDto input)
        {
            SelectedEditTab = "order-edit-tab";
            
            
            var order = await OrdersAppService.GetAsync(input.Id);
            
            EditingOrderId = order.Id;
            EditingOrder = ObjectMapper.Map<OrderDto, OrderUpdateDto>(order);
            
            await EditingOrderValidations.ClearAll();
            await EditOrderModal.Show();
        }

        private async Task DeleteOrderAsync(OrderDto input)
        {
            await OrdersAppService.DeleteAsync(input.Id);
            await GetOrdersAsync();
        }

        private async Task CreateOrderAsync()
        {
            try
            {
                if (await NewOrderValidations.ValidateAll() == false)
                {
                    return;
                }

                await OrdersAppService.CreateAsync(NewOrder);
                await GetOrdersAsync();
                await CloseCreateOrderModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditOrderModalAsync()
        {
            await EditOrderModal.Hide();
        }

        private async Task UpdateOrderAsync()
        {
            try
            {
                if (await EditingOrderValidations.ValidateAll() == false)
                {
                    return;
                }

                await OrdersAppService.UpdateAsync(EditingOrderId, EditingOrder);
                await GetOrdersAsync();
                await EditOrderModal.Hide();                
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private void OnSelectedCreateTabChanged(string name)
        {
            SelectedCreateTab = name;
        }

        private void OnSelectedEditTabChanged(string name)
        {
            SelectedEditTab = name;
        }









        protected virtual async Task OnUserIdChangedAsync(string? userId)
        {
            Filter.UserId = userId;
            await SearchAsync();
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
        







    }
}
