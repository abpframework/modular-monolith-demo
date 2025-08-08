using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Shopularity.Catalog.Categories;
using Shopularity.Catalog.Products;
using SixLabors.ImageSharp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Shopularity.Catalog.Data;

public class CatalogDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly ProductManager _productManager;
    private readonly IProductRepository _productRepository;
    private readonly CategoryManager _categoryManager;
    private readonly ICategoryRepository _categoryRepository;

    public CatalogDataSeedContributor(
        ProductManager productManager,
        IProductRepository productRepository,
        CategoryManager categoryManager,
        ICategoryRepository categoryRepository)
    {
        _productManager = productManager;
        _productRepository = productRepository;
        _categoryManager = categoryManager;
        _categoryRepository = categoryRepository;
    }
    
    public async Task SeedAsync(DataSeedContext context)
    {
        await SeedElectronicsAsync();
        await SeedClothingAsync();
        await SeedKitchenAsync();
        await SeedPersonalCareAsync();
    }

    private async Task SeedPersonalCareAsync()
    {
        string productName;
        var categoryId = await GetCategoryIdAsync("Personal Care");

        productName = "Manicure Set Professional Nail Clipper";
        if (!await _productRepository.AnyAsync(x => x.Name == productName))
        {
            var image = await DownloadImageAsBytesAsync("https://m.media-amazon.com/images/I/812JNL02tAL._AC_SX679_PIbundle-26,TopRight,0,0_SH20_.jpg");
            await _productManager.CreateAsync(
                categoryId,
                productName,
                9.98,
                100,
                image,
                "Manicure Set：professional manicure kit contains 26 tools,multi-functional include hand care,facial care,foot care tools."
            );
        }

        productName = "Facial Steamer Spa Kit";
        if (!await _productRepository.AnyAsync(x => x.Name == productName))
        {
            var image = await DownloadImageAsBytesAsync("https://m.media-amazon.com/images/I/71rSPyVkyAL._AC_SX679_.jpg");
            await _productManager.CreateAsync(
                categoryId,
                productName,
                19.99,
                100,
                image,
                "Spa-Like Experience with one-of-a-kind Facial Steamer Spa Kit - Indulge in a refreshing, relaxing and rejuvenating spa-like experience at home to get your dream skin with a 9-step regime."
            );
        }

        productName = "Philips Sonicare Genuine C2";
        if (!await _productRepository.AnyAsync(x => x.Name == productName))
        {
            var image = await DownloadImageAsBytesAsync("https://m.media-amazon.com/images/I/71YsyR9MpGL._AC_SX679_.jpg");
            await _productManager.CreateAsync(
                categoryId,
                productName,
                19.99,
                100,
                image,
                "Genuine Philips Sonicare rechargeable electric toothbrush head compatible with all Philips Sonicare click-on rechargeable toothbrush handles."
            );
        }
    }

    private async Task SeedKitchenAsync()
    {
        string productName;
        var categoryId = await GetCategoryIdAsync("Kitchen");

        productName = "Over The Sink Dish Drying Rack";
        if (!await _productRepository.AnyAsync(x => x.Name == productName))
        {
            var image = await DownloadImageAsBytesAsync("https://m.media-amazon.com/images/I/71sU8KRIAzL._AC_SX679_.jpg");
            await _productManager.CreateAsync(
                categoryId,
                productName,
                27.00,
                100,
                image,
                "The dimension of our kitchen sink organizers is 17.5\" (L) x 11.8\" (W). Suitable for kitchen sink, undermount kitchen sink, rv sink, camping sink and other outdoor kitchen sinks, etc. But please measure your sink or kitchen counter FIRST before purchasing this over sink dish drying rack, then you will get the perfect kitchen sink organization and storage tools, kitchen accessories for countertop, rv accessories, apartment necessities, camping kitchen sink accessories, etc."
            );
        }

        productName = "Maifan Sink Caddy Sponge Holder";
        if (!await _productRepository.AnyAsync(x => x.Name == productName))
        {
            var image = await DownloadImageAsBytesAsync("https://m.media-amazon.com/images/I/71rSPyVkyAL._AC_SX679_.jpg");
            await _productManager.CreateAsync(
                categoryId,
                productName,
                13.99,
                100,
                image,
                "Our Sponge Holder are sloped and self-draining to help save time on repeated sponge washes, reducing the workload in the kitchen and ensuring that your countertops stay dry and clean."
            );
        }
    }

    private async Task SeedClothingAsync()
    {
        string productName;
        var categoryId = await GetCategoryIdAsync("Clothing");

        productName = "Gloria Vanderbilt";
        if (!await _productRepository.AnyAsync(x => x.Name == productName))
        {
            var image = await DownloadImageAsBytesAsync("https://m.media-amazon.com/images/I/51fLTno0lJL._AC_SX569_.jpg");
            await _productManager.CreateAsync(
                categoryId,
                productName,
                27.00,
                100,
                image,
                "Flattering High-Waisted Fit Women Pants: Gloria Vanderbilt Amanda Stretch Denim Jeans offer a classic high waisted jeans for women design for a slimming and chic look. These women's pants are perfect for all body types."
            );
        }

        productName = "Avoogue Raincoat";
        if (!await _productRepository.AnyAsync(x => x.Name == productName))
        {
            var image = await DownloadImageAsBytesAsync("https://m.media-amazon.com/images/I/71vI46FOSwL._AC_SY550_.jpg");
            await _productManager.CreateAsync(
                categoryId,
                productName,
                31.99,
                100,
                image,
                "WATERPROOF& WINDPROOF RAINCOAT: This hooded rain jacket is made of super waterproof material, it's lightweight , breathable , packable , skin-friendly, comfortable to wear, It will keep you dry and cool in every rainy days."
            );
        }

        productName = "Gildan Unisex Adult Ultra Cotton T-Shirt";
        if (!await _productRepository.AnyAsync(x => x.Name == productName))
        {
            var image = await DownloadImageAsBytesAsync("https://m.media-amazon.com/images/I/61x-LF9fyYL._AC_SX425_.jpg");
            await _productManager.CreateAsync(
                categoryId,
                productName,
                9.99,
                100,
                image,
                "Whether wearing it for work or play, the Gildan Ultra Cotton Adult T-Shirt is a wardrobe basic that works for just about any occasion. It features a classic fit and a heavyweight fabric with taped neck and shoulders for comfort and durability. You can decorate using screen printing, DTG, heat transfers, dye sublimation or discharge, and companion styles for youth and toddlers make it easy to create matching outfits for the whole family."
            );
        }
    }

    private async Task SeedElectronicsAsync()
    {
        string productName;
        var electronicsCategoryId = await GetCategoryIdAsync("Electronics");

        productName = "Logitech G305";
        if (!await _productRepository.AnyAsync(x => x.Name == productName))
        {
            var image = await DownloadImageAsBytesAsync("https://m.media-amazon.com/images/I/61A3HlkJOOL._AC_SL1500_.jpg");
            await _productManager.CreateAsync(
                electronicsCategoryId,
                productName,
                199.99,
                100,
                image,
                "Play advanced without wires or limits. The Logitech G305 LIGHTSPEED is a gaming wireless mouse designed for high-performance in your favorite PC games."
            );
        }

        productName = "BENGOO G9000";
        if (!await _productRepository.AnyAsync(x => x.Name == productName))
        {
            var image = await DownloadImageAsBytesAsync("https://m.media-amazon.com/images/I/61CGHv6kmWL._AC_SL1000_.jpg");
            await _productManager.CreateAsync(
                electronicsCategoryId,
                productName,
                19.99,
                100,
                image,
                "Clear sound operating strong brass, splendid ambient noise isolation and high precision 40mm magnetic neodymium driver, acoustic positioning precision enhance the sensitivity of the speaker unit, bringing you vivid sound field, sound clarity, shock feeling sound."
            );
        }

        productName = "Papeaso HU-03C";
        if (!await _productRepository.AnyAsync(x => x.Name == productName))
        {
            var image = await DownloadImageAsBytesAsync("https://m.media-amazon.com/images/I/61+O33qy3nL._AC_SL1500_.jpg");
            await _productManager.CreateAsync(
                electronicsCategoryId,
                productName,
                19.99,
                100,
                image,
                "Video Capture Card, Video Recording Card Wonderful 4K HDMI to 1080P USB C for Gaming Streaming TV Recorder, for Windows Mac OS System with USB C Adapter to Adjust PS4, Switch, Xbox1."
            );
        }

        productName = "GEODMAER GM68";
        if (!await _productRepository.AnyAsync(x => x.Name == productName))
        {
            var image = await DownloadImageAsBytesAsync("https://m.media-amazon.com/images/I/71FG5DVmBdL._AC_SL1500_.jpg");
            await _productManager.CreateAsync(
                electronicsCategoryId,
                productName,
                19.99,
                100,
                image,
                "GEODMAER Wired gaming keyboard compact mini design, save space on the desktop, novel black & silver gray keycap color matching, separate arrow keys, No numpad, both gaming and office, easy to carry size can be easily put into the backpack."
            );
        }
    }

    private async Task<Guid> GetCategoryIdAsync(string categoryName)
    {
        var categoryId = (await _categoryRepository.FirstOrDefaultAsync(x=> x.Name == categoryName))?.Id;
        
        if (categoryId == null)
        {
            var category = await _categoryManager.CreateAsync(categoryName);
            return category.Id;
        }

        return categoryId.Value;
    }
    
    public async Task<byte[]> DownloadImageAsBytesAsync(string imageUrl)
    {
        using var httpClient = new HttpClient();
        using var response = await httpClient.GetAsync(imageUrl);
        await using var stream = await response.Content.ReadAsStreamAsync();
        using var image = await Image.LoadAsync(stream);
        using var ms = new MemoryStream();
        await image.SaveAsPngAsync(ms);
        return ms.ToArray();
    }
}