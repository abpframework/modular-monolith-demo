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
using Shopularity.Catalog.Categories;
using Shopularity.Catalog.Permissions;
using Shopularity.Catalog.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Volo.Abp;
using Volo.Abp.Content;

namespace Shopularity.Catalog.Blazor.Pages.Catalog
{
    public partial class Categories
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        protected bool ShowAdvancedFilters { get; set; }
        public DataGrid<CategoryDto> DataGridRef { get; set; }
        private IReadOnlyList<CategoryDto> CategoryList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateCategory { get; set; }
        private bool CanEditCategory { get; set; }
        private bool CanDeleteCategory { get; set; }
        private CategoryCreateDto NewCategory { get; set; }
        private Validations NewCategoryValidations { get; set; } = new();
        private CategoryUpdateDto EditingCategory { get; set; }
        private Validations EditingCategoryValidations { get; set; } = new();
        private Guid EditingCategoryId { get; set; }
        private Modal CreateCategoryModal { get; set; } = new();
        private Modal EditCategoryModal { get; set; } = new();
        private GetCategoriesInput Filter { get; set; }
        private DataGridEntityActionsColumn<CategoryDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "category-create-tab";
        protected string SelectedEditTab = "category-edit-tab";
        private CategoryDto? SelectedCategory;
        
        public Categories()
        {
            NewCategory = new CategoryCreateDto();
            EditingCategory = new CategoryUpdateDto();
            Filter = new GetCategoriesInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            CategoryList = new List<CategoryDto>();
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
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Categories"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["NewCategory"], async () =>
            {
                await OpenCreateCategoryModalAsync();
            }, IconName.Add, requiredPolicyName: CatalogPermissions.Categories.Create);

            return ValueTask.CompletedTask;
        }
        
        private void ToggleDetails(CategoryDto category)
        {
            DataGridRef.ToggleDetailRow(category, true);
        }
        
        private bool RowSelectableHandler( RowSelectableEventArgs<CategoryDto> rowSelectableEventArgs )
            => rowSelectableEventArgs.SelectReason is not DataGridSelectReason.RowClick && CanDeleteCategory;
            
        private bool DetailRowTriggerHandler(DetailRowTriggerEventArgs<CategoryDto> detailRowTriggerEventArgs)
        {
            detailRowTriggerEventArgs.Toggleable = false;
            detailRowTriggerEventArgs.DetailRowTriggerType = DetailRowTriggerType.Manual;
            return true;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateCategory = await AuthorizationService
                .IsGrantedAsync(CatalogPermissions.Categories.Create);
            CanEditCategory = await AuthorizationService
                            .IsGrantedAsync(CatalogPermissions.Categories.Edit);
            CanDeleteCategory = await AuthorizationService
                            .IsGrantedAsync(CatalogPermissions.Categories.Delete);
        }

        private async Task GetCategoriesAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await CategoriesAppService.GetListAsync(Filter);
            CategoryList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetCategoriesAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<CategoryDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetCategoriesAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateCategoryModalAsync()
        {
            NewCategory = new CategoryCreateDto();

            SelectedCreateTab = "category-create-tab";
            
            await NewCategoryValidations.ClearAll();
            await CreateCategoryModal.Show();
        }

        private async Task CloseCreateCategoryModalAsync()
        {
            NewCategory = new CategoryCreateDto();
            await CreateCategoryModal.Hide();
        }

        private async Task OpenEditCategoryModalAsync(CategoryDto input)
        {
            SelectedEditTab = "category-edit-tab";
            
            var category = await CategoriesAppService.GetAsync(input.Id);
            
            EditingCategoryId = category.Id;
            EditingCategory = ObjectMapper.Map<CategoryDto, CategoryUpdateDto>(category);
            
            await EditingCategoryValidations.ClearAll();
            await EditCategoryModal.Show();
        }

        private async Task DeleteCategoryAsync(CategoryDto input)
        {
            await CategoriesAppService.DeleteAsync(input.Id);
            await GetCategoriesAsync();
        }

        private async Task CreateCategoryAsync()
        {
            try
            {
                if (await NewCategoryValidations.ValidateAll() == false)
                {
                    return;
                }

                await CategoriesAppService.CreateAsync(NewCategory);
                await GetCategoriesAsync();
                await CloseCreateCategoryModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditCategoryModalAsync()
        {
            await EditCategoryModal.Hide();
        }

        private async Task UpdateCategoryAsync()
        {
            try
            {
                if (await EditingCategoryValidations.ValidateAll() == false)
                {
                    return;
                }

                await CategoriesAppService.UpdateAsync(EditingCategoryId, EditingCategory);
                await GetCategoriesAsync();
                await EditCategoryModal.Hide();                
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
    }
}
