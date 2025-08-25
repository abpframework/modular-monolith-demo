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
using Volo.Abp.Caching;
using DistributedCacheEntryOptions = Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions;

namespace Shopularity.Public.Pages;

public class CheckOutModel : ShopularityPublicPageModel
{
    public IBasketAppService BasketAppService { get; }
    public IProductsPublicAppService ProductsPublicAppService { get; }
    public IShopularityAppService ShopularityAppService { get; }
    public IDistributedCache<BasketCheckoutCacheItem> Cache { get; }

    [HiddenInput] [BindProperty] public string CacheId { get; set; }

    public List<BasketViewItemModel> Items { get; set; }

    public double TotalPrice { get; set; }

    [BindProperty] public string CreditCardNumber { get; set; }

    [TextArea] [BindProperty] public string Address { get; set; }

    public CheckOutModel(
        IBasketAppService basketAppService,
        IProductsPublicAppService productsPublicAppService,
        IShopularityAppService shopularityAppService,
        IDistributedCache<BasketCheckoutCacheItem> cache)
    {
        BasketAppService = basketAppService;
        ProductsPublicAppService = productsPublicAppService;
        ShopularityAppService = shopularityAppService;
        Cache = cache;
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
            Ids = result.Select(x => x.ItemId).ToList()
        });

        Items = productsWithDetails.Items.Select(x => new BasketViewItemModel
        {
            Product = x.Product,
            Category = x.Category,
            Amount = result.FirstOrDefault(y => x.Product.Id == y.ItemId)?.Amount ?? 1,
        }).ToList();

        TotalPrice = Items.Select(x => x.Product.Price * x.Amount).Sum();

        CacheId = GuidGenerator.Create().ToString();
        await Cache.SetAsync(CacheId, new BasketCheckoutCacheItem
            {
                Items = Items,
                TotalPrice = TotalPrice
            },
            new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15)
            });

        return Page();
    }

    public virtual async Task<ActionResult> OnPostAsync()
    {
        var basketFromCache = await Cache.GetAsync(CacheId);
        
        if (basketFromCache == null)
        {
            throw new UserFriendlyException("Time out! Please refresh the page to continue checking-out.");
        }

        await ShopularityAppService.CreateOrderAsync(new NewOrderInputDto
        {
            Address = Address,
            CreditCardNo = CreditCardNumber,
            Products = basketFromCache.Items.Select(x => new BasketItem { ItemId = x.Product.Id, Amount = x.Amount })
                .ToList()
        });

        return Redirect("/my-orders");
    }

    public class BasketCheckoutCacheItem
    {
        public List<BasketViewItemModel> Items { get; set; }

        public double TotalPrice { get; set; }
    }
}