using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Basket.Services;
using Shopularity.Catalog.Products;
using Shopularity.Catalog.Products.Public;
using Shopularity.Ordering.Orders.Public;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Caching;
using DistributedCacheEntryOptions = Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions;

namespace Shopularity.Public.Pages;

public class CheckOutModel : ShopularityPublicPageModel
{
    public IBasketAppService BasketAppService { get; }
    public IProductsPublicAppService ProductsPublicAppService { get; }
    public IOrdersPublicAppService OrdersPublicAppService { get; }
    public IDistributedCache<BasketCheckoutCacheItem> Cache { get; }

    [HiddenInput] [BindProperty] public string CacheId { get; set; }

    public List<BasketCacheItemModel> Items { get; set; } = new();

    public double TotalPrice { get; set; }

    [BindProperty] public string CreditCardNumber { get; set; }

    [TextArea] [BindProperty] public string Address { get; set; }

    public CheckOutModel(
        IBasketAppService basketAppService,
        IProductsPublicAppService productsPublicAppService,
        IOrdersPublicAppService ordersPublicAppService,
        IDistributedCache<BasketCheckoutCacheItem> cache)
    {
        BasketAppService = basketAppService;
        ProductsPublicAppService = productsPublicAppService;
        OrdersPublicAppService = ordersPublicAppService;
        Cache = cache;
    }

    public virtual async Task<ActionResult> OnGetAsync()
    {
        var basket = await BasketAppService.GetBasketItemsAsync();

        if (!basket.Items.Any())
        {
            return Redirect("/order-history");
        }

        foreach (var item in basket.Items)
        {
            var newItem = ObjectMapper.Map<ProductPublicDto, BasketCacheItemModel>(item.Product);
            newItem.Amount = item.Amount;
            Items.Add(newItem);
        }

        TotalPrice = Items.Select(x => x.Price * x.Amount).Sum();

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
        
        await OrdersPublicAppService.CreateAsync(new OrderCreatePublicDto
        {
            ShippingAddress = Address,
            Products = basketFromCache.Items.Select(x=> new ProductIdsWithAmountDto
            {
                ProductId = x.Id,
                Amount = x.Amount
            }).ToList(),
        });

        return Redirect("/order-history");
    }

    public class BasketCheckoutCacheItem
    {
        public List<BasketCacheItemModel> Items { get; set; }

        public double TotalPrice { get; set; }
    }

    public class BasketCacheItemModel
    {
        public Guid Id { get; set; }
    
        public string Name { get; set; } = null!;
        
        public string? Description { get; set; }
        
        public double Price { get; set; }
    
        public int Amount { get; set; }
    }
}