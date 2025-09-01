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
using Shopularity.Catalog.Products;
using Shopularity.Catalog.Permissions;
using Shopularity.Catalog.Shared;
using Shopularity.Catalog.Localization;
using Shopularity.Catalog.Products.Admin;

namespace Shopularity.Catalog.Blazor.Pages.Catalog;

public partial class Products
{
    protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
    protected PageToolbar Toolbar {get;} = new PageToolbar();
    protected bool ShowAdvancedFilters { get; set; }
    public DataGrid<ProductWithNavigationPropertiesDto> DataGridRef { get; set; }
    private IReadOnlyList<ProductWithNavigationPropertiesDto> ProductList { get; set; }
    private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
    private int CurrentPage { get; set; } = 1;
    private string CurrentSorting { get; set; } = string.Empty;
    private int TotalCount { get; set; }
    private bool CanCreateProduct { get; set; }
    private bool CanEditProduct { get; set; }
    private bool CanDeleteProduct { get; set; }
    private ProductCreateDto NewProduct { get; set; }
    private Validations NewProductValidations { get; set; } = new();
    private ProductUpdateDto EditingProduct { get; set; }
    private Validations EditingProductValidations { get; set; } = new();
    private Guid EditingProductId { get; set; }
    private Modal CreateProductModal { get; set; } = new();
    private Modal EditProductModal { get; set; } = new();
    private GetProductsInput Filter { get; set; }
    private DataGridEntityActionsColumn<ProductWithNavigationPropertiesDto> EntityActionsColumn { get; set; } = new();
    protected string SelectedCreateTab = "product-create-tab";
    protected string SelectedEditTab = "product-edit-tab";
    private ProductWithNavigationPropertiesDto? SelectedProduct;
    private IReadOnlyList<LookupDto<Guid>> CategoriesCollection { get; set; } = new List<LookupDto<Guid>>();
    private List<ProductWithNavigationPropertiesDto> SelectedProducts { get; set; } = new();
    private bool AllProductsSelected { get; set; }
        
    public Products()
    {
        LocalizationResource = typeof(CatalogResource);
            
        NewProduct = new ProductCreateDto();
        EditingProduct = new ProductUpdateDto();
        Filter = new GetProductsInput
        {
            MaxResultCount = PageSize,
            SkipCount = (CurrentPage - 1) * PageSize,
            Sorting = CurrentSorting
        };
        ProductList = new List<ProductWithNavigationPropertiesDto>();
    }

    protected override async Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
        await GetCategoryCollectionLookupAsync();
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
        BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Products"]));
        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
        Toolbar.AddButton(L["NewProduct"], async () =>
        {
            await OpenCreateProductModalAsync();
        }, IconName.Add, requiredPolicyName: CatalogPermissions.Products.Create);

        return ValueTask.CompletedTask;
    }
        
    private bool RowSelectableHandler( RowSelectableEventArgs<ProductWithNavigationPropertiesDto> rowSelectableEventArgs )
        => rowSelectableEventArgs.SelectReason is not DataGridSelectReason.RowClick && CanDeleteProduct;

    private async Task SetPermissionsAsync()
    {
        CanCreateProduct = await AuthorizationService
            .IsGrantedAsync(CatalogPermissions.Products.Create);
        CanEditProduct = await AuthorizationService
            .IsGrantedAsync(CatalogPermissions.Products.Edit);
        CanDeleteProduct = await AuthorizationService
            .IsGrantedAsync(CatalogPermissions.Products.Delete);
    }

    private async Task GetProductsAsync()
    {
        Filter.MaxResultCount = PageSize;
        Filter.SkipCount = (CurrentPage - 1) * PageSize;
        Filter.Sorting = CurrentSorting;

        var result = await ProductsAdminAppService.GetListAsync(Filter);
        ProductList = result.Items;
        TotalCount = (int)result.TotalCount;
            
        await ClearSelection();
    }

    protected virtual async Task SearchAsync()
    {
        CurrentPage = 1;
        await GetProductsAsync();
        await InvokeAsync(StateHasChanged);
    }

    private async Task DownloadAsExcelAsync()
    {
        var token = (await ProductsAdminAppService.GetDownloadTokenAsync()).Token;
        var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Catalog") ?? await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
        var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
        if(!culture.IsNullOrEmpty())
        {
            culture = "&culture=" + culture;
        }
        await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
        NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/catalog/products/as-excel-file?DownloadToken={token}&FilterText={HttpUtility.UrlEncode(Filter.FilterText)}{culture}&Name={HttpUtility.UrlEncode(Filter.Name)}&Description={HttpUtility.UrlEncode(Filter.Description)}&PriceMin={Filter.PriceMin}&PriceMax={Filter.PriceMax}&StockCountMin={Filter.StockCountMin}&StockCountMax={Filter.StockCountMax}&CategoryId={Filter.CategoryId}", forceLoad: true);
    }

    private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<ProductWithNavigationPropertiesDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page;
        await GetProductsAsync();
        await InvokeAsync(StateHasChanged);
    }

    private async Task OpenCreateProductModalAsync()
    {
        NewProduct = new ProductCreateDto();

        SelectedCreateTab = "product-create-tab";
            
        await NewProductValidations.ClearAll();
        await CreateProductModal.Show();
    }

    private async Task CloseCreateProductModalAsync()
    {
        NewProduct = new ProductCreateDto();
        await CreateProductModal.Hide();
    }

    private async Task OpenEditProductModalAsync(ProductWithNavigationPropertiesDto input)
    {
        SelectedEditTab = "product-edit-tab";
            
        var product = await ProductsAdminAppService.GetWithNavigationPropertiesAsync(input.Product.Id);
            
        EditingProductId = product.Product.Id;
        EditingProduct = ObjectMapper.Map<ProductDto, ProductUpdateDto>(product.Product);
            
        await EditingProductValidations.ClearAll();
        await EditProductModal.Show();
    }

    private async Task DeleteProductAsync(ProductWithNavigationPropertiesDto input)
    {
        await ProductsAdminAppService.DeleteAsync(input.Product.Id);
        await GetProductsAsync();
    }

    private async Task CreateProductAsync()
    {
        try
        {
            if (await NewProductValidations.ValidateAll() == false)
            {
                return;
            }

            await ProductsAdminAppService.CreateAsync(NewProduct);
            await GetProductsAsync();
            await CloseCreateProductModalAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    private async Task CloseEditProductModalAsync()
    {
        await EditProductModal.Hide();
    }

    private async Task UpdateProductAsync()
    {
        try
        {
            if (await EditingProductValidations.ValidateAll() == false)
            {
                return;
            }

            await ProductsAdminAppService.UpdateAsync(EditingProductId, EditingProduct);
            await GetProductsAsync();
            await EditProductModal.Hide();                
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

    protected virtual async Task OnNameChangedAsync(string? name)
    {
        Filter.Name = name;
        await SearchAsync();
    }
    protected virtual async Task OnDescriptionChangedAsync(string? description)
    {
        Filter.Description = description;
        await SearchAsync();
    }
    protected virtual async Task OnPriceMinChangedAsync(double? priceMin)
    {
        Filter.PriceMin = priceMin;
        await SearchAsync();
    }
    protected virtual async Task OnPriceMaxChangedAsync(double? priceMax)
    {
        Filter.PriceMax = priceMax;
        await SearchAsync();
    }
    protected virtual async Task OnStockCountMinChangedAsync(int? stockCountMin)
    {
        Filter.StockCountMin = stockCountMin;
        await SearchAsync();
    }
    protected virtual async Task OnStockCountMaxChangedAsync(int? stockCountMax)
    {
        Filter.StockCountMax = stockCountMax;
        await SearchAsync();
    }
    protected virtual async Task OnCategoryIdChangedAsync(Guid? categoryId)
    {
        Filter.CategoryId = categoryId;
        await SearchAsync();
    }

    private async Task GetCategoryCollectionLookupAsync(string? newValue = null)
    {
        CategoriesCollection = (await ProductsAdminAppService.GetCategoryLookupAsync(new LookupRequestDto { Filter = newValue })).Items;
    }

    private Task SelectAllItems()
    {
        AllProductsSelected = true;
            
        return Task.CompletedTask;
    }

    private Task ClearSelection()
    {
        AllProductsSelected = false;
        SelectedProducts.Clear();
            
        return Task.CompletedTask;
    }

    private Task SelectedProductRowsChanged()
    {
        if (SelectedProducts.Count != PageSize)
        {
            AllProductsSelected = false;
        }
            
        return Task.CompletedTask;
    }

    private async Task DeleteSelectedProductsAsync()
    {
        var message = AllProductsSelected ? L["DeleteAllRecords"].Value : L["DeleteSelectedRecords", SelectedProducts.Count].Value;
            
        if (!await UiMessageService.Confirm(message))
        {
            return;
        }

        if (AllProductsSelected)
        {
            await ProductsAdminAppService.DeleteAllAsync(Filter);
        }
        else
        {
            await ProductsAdminAppService.DeleteByIdsAsync(SelectedProducts.Select(x => x.Product.Id).ToList());
        }

        SelectedProducts.Clear();
        AllProductsSelected = false;

        await GetProductsAsync();
    }
        
    private async Task OnFileChanged(FileChangedEventArgs e)
    {
        var file = e.Files.FirstOrDefault();
        if (file is null)
        {
            return;
        }

        using var stream = file.OpenReadStream(16 * 1024 * 1024);
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        EditingProduct.Image = ms.ToArray();
    }
        
    private async Task ClearEditingProductImage()
    {
        EditingProduct.Image = null;
    }
}