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
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Shopularity.Payment.Payments;
using Shopularity.Payment.Permissions;

namespace Shopularity.Payment.Blazor.Pages.Payment
{
    public partial class Payments
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        protected bool ShowAdvancedFilters { get; set; }
        public DataGrid<PaymentDto> DataGridRef { get; set; }
        private IReadOnlyList<PaymentDto> PaymentList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreatePayment { get; set; }
        private bool CanEditPayment { get; set; }
        private bool CanDeletePayment { get; set; }
        private PaymentCreateDto NewPayment { get; set; }
        private Validations NewPaymentValidations { get; set; } = new();
        private PaymentUpdateDto EditingPayment { get; set; }
        private Validations EditingPaymentValidations { get; set; } = new();
        private Guid EditingPaymentId { get; set; }
        private Modal CreatePaymentModal { get; set; } = new();
        private Modal EditPaymentModal { get; set; } = new();
        private GetPaymentsInput Filter { get; set; }
        private DataGridEntityActionsColumn<PaymentDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "payment-create-tab";
        protected string SelectedEditTab = "payment-edit-tab";
        private PaymentDto? SelectedPayment;
        
        public Payments()
        {
            LocalizationResource = typeof(Localization.PaymentResource);
            
            NewPayment = new PaymentCreateDto();
            EditingPayment = new PaymentUpdateDto();
            Filter = new GetPaymentsInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            PaymentList = new List<PaymentDto>();
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
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Payments"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewPayment"], async () =>
            {
                await OpenCreatePaymentModalAsync();
            }, IconName.Add, requiredPolicyName: PaymentPermissions.Payments.Create);

            return ValueTask.CompletedTask;
        }
        
        private void ToggleDetails(PaymentDto payment)
        {
            DataGridRef.ToggleDetailRow(payment, true);
        }
        
        private bool RowSelectableHandler( RowSelectableEventArgs<PaymentDto> rowSelectableEventArgs )
            => rowSelectableEventArgs.SelectReason is not DataGridSelectReason.RowClick && CanDeletePayment;
            
        private bool DetailRowTriggerHandler(DetailRowTriggerEventArgs<PaymentDto> detailRowTriggerEventArgs)
        {
            detailRowTriggerEventArgs.Toggleable = false;
            detailRowTriggerEventArgs.DetailRowTriggerType = DetailRowTriggerType.Manual;
            return true;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreatePayment = await AuthorizationService
                .IsGrantedAsync(PaymentPermissions.Payments.Create);
            CanEditPayment = await AuthorizationService
                            .IsGrantedAsync(PaymentPermissions.Payments.Edit);
            CanDeletePayment = await AuthorizationService
                            .IsGrantedAsync(PaymentPermissions.Payments.Delete);
        }

        private async Task GetPaymentsAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await PaymentsAppService.GetListAsync(Filter);
            PaymentList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetPaymentsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task DownloadAsExcelAsync()
        {
            var token = (await PaymentsAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Payment") ?? await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if(!culture.IsNullOrEmpty())
            {
                culture = "&culture=" + culture;
            }
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/payment/payments/as-excel-file?DownloadToken={token}&FilterText={HttpUtility.UrlEncode(Filter.FilterText)}{culture}&OrderId={HttpUtility.UrlEncode(Filter.OrderId)}&State={Filter.State}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<PaymentDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetPaymentsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreatePaymentModalAsync()
        {
            NewPayment = new PaymentCreateDto();

            SelectedCreateTab = "payment-create-tab";
            
            
            await NewPaymentValidations.ClearAll();
            await CreatePaymentModal.Show();
        }

        private async Task CloseCreatePaymentModalAsync()
        {
            NewPayment = new PaymentCreateDto();
            await CreatePaymentModal.Hide();
        }

        private async Task OpenEditPaymentModalAsync(PaymentDto input)
        {
            SelectedEditTab = "payment-edit-tab";
            
            var payment = await PaymentsAppService.GetAsync(input.Id);
            
            EditingPaymentId = payment.Id;
            EditingPayment = ObjectMapper.Map<PaymentDto, PaymentUpdateDto>(payment);
            
            await EditingPaymentValidations.ClearAll();
            await EditPaymentModal.Show();
        }

        private async Task DeletePaymentAsync(PaymentDto input)
        {
            await PaymentsAppService.DeleteAsync(input.Id);
            await GetPaymentsAsync();
        }

        private async Task CreatePaymentAsync()
        {
            try
            {
                if (await NewPaymentValidations.ValidateAll() == false)
                {
                    return;
                }

                await PaymentsAppService.CreateAsync(NewPayment);
                await GetPaymentsAsync();
                await CloseCreatePaymentModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditPaymentModalAsync()
        {
            await EditPaymentModal.Hide();
        }

        private async Task UpdatePaymentAsync()
        {
            try
            {
                if (await EditingPaymentValidations.ValidateAll() == false)
                {
                    return;
                }

                await PaymentsAppService.UpdateAsync(EditingPaymentId, EditingPayment);
                await GetPaymentsAsync();
                await EditPaymentModal.Hide();                
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

        protected virtual async Task OnOrderIdChangedAsync(string? orderId)
        {
            Filter.OrderId = orderId;
            await SearchAsync();
        }
        protected virtual async Task OnStateChangedAsync(PaymentState? state)
        {
            Filter.State = state;
            await SearchAsync();
        }
    }
}
