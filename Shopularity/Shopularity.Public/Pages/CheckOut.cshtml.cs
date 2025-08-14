using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Shopularity.Basket.Services;
using Shopularity.Catalog.Products;
using Shopularity.Catalog.Products.Public;
using Shopularity.Public.Components.Basket;
using Shopularity.Services.Dtos;
using Shopularity.Services.Orders;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Shopularity.Public.Pages;

public class CheckOutModel : ShopularityPublicPageModel
{
    public IBasketAppService BasketAppService { get; }
    public IProductsPublicAppService ProductsPublicAppService { get; }
    public IOrderingPublicAppService OrderingPublicAppService { get; }
    public IMemoryCache MemoryCache { get; }

    [HiddenInput]
    [BindProperty]
    public string CacheId { get; set; }
    
    public List<BasketViewItemModel> Items { get; set; }
    
    public double TotalPrice { get; set; }
    
    [BindProperty]
    public string CreditCardNumber { get; set; }
    
    [TextArea]
    [BindProperty]
    public string Address { get; set; }

    public CheckOutModel(
        IBasketAppService basketAppService,
        IProductsPublicAppService productsPublicAppService,
        IOrderingPublicAppService orderingPublicAppService,
        IMemoryCache memoryCache)
    {
        BasketAppService = basketAppService;
        ProductsPublicAppService = productsPublicAppService;
        OrderingPublicAppService = orderingPublicAppService;
        MemoryCache = memoryCache;
    }
    
    public virtual async Task<ActionResult> OnGetAsync()
    {
        var result = await BasketAppService.GetBasketItems();

        if (!result.Any())
        {
            Items = new List<BasketViewItemModel>();
        }

        var productsWithDetails = await ProductsPublicAppService.GetListByIdsAsync(new GetListByIdsInput
        {
            Ids = result.Select(x => x.ProductId).ToList()
        });

        Items = productsWithDetails.Items.Select(x => new BasketViewItemModel
        {
            Product = x.Product,
            Category = x.Category,
            Amount = result.FirstOrDefault(y => x.Product.Id == y.ProductId)?.Amount ?? 1,
        }).ToList();

        TotalPrice = Items.Select(x => x.Product.Price * x.Amount).Sum();

        CacheId = GuidGenerator.Create().ToString();
        MemoryCache.Set(CacheId, new BasketCheckoutCacheItem
        {
            Items = Items,
            TotalPrice = TotalPrice
        }, absoluteExpirationRelativeToNow: TimeSpan.FromMinutes(1));
        
        return Page();
    }

    public virtual async Task<ActionResult> OnPostAsync()
    {
        MemoryCache.TryGetValue(CacheId, out BasketCheckoutCacheItem? basketFromCache);

        if (basketFromCache == null)
        {
            throw new UserFriendlyException("Time out! Please refresh the page to continue checking-out.");
        }

        await OrderingPublicAppService.CreateOrderAsync(new NewOrderInputDto
        {
            Address = Address,
            CreditCardNo = CreditCardNumber,
            Products = basketFromCache.Items.Select(x=> new BasketItem{ ProductId = x.Product.Id, Amount = x.Amount}).ToList()
        });
        
        return RedirectToPage("/my-orders");
    }

    internal class BasketCheckoutCacheItem
    {
        public List<BasketViewItemModel> Items { get; set; }
    
        public double TotalPrice { get; set; }
    }
}