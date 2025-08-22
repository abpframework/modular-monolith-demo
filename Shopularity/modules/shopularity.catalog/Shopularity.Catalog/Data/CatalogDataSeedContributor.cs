using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Shopularity.Catalog.Categories;
using Shopularity.Catalog.Products;
using SixLabors.ImageSharp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Shopularity.Catalog.Data;

public class CatalogDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private const int DummyProductCount = 1000;
    
    private readonly ProductManager _productManager;
    private readonly IProductRepository _productRepository;
    private readonly CategoryManager _categoryManager;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWorkAccessor _unitOfWorkAccessor;
    private readonly IHttpClientFactory _clientFactory;

    public CatalogDataSeedContributor(
        ProductManager productManager,
        IProductRepository productRepository,
        CategoryManager categoryManager,
        ICategoryRepository categoryRepository,
        IUnitOfWorkAccessor unitOfWorkAccessor,
        IHttpClientFactory clientFactory)
    {
        _productManager = productManager;
        _productRepository = productRepository;
        _categoryManager = categoryManager;
        _categoryRepository = categoryRepository;
        _unitOfWorkAccessor = unitOfWorkAccessor;
        _clientFactory = clientFactory;
    }
    
    public async Task SeedAsync(DataSeedContext context)
    {
        var client = _clientFactory.CreateClient();
        var dummyProducts = await client.GetFromJsonAsync<DummyProductsResponse>($"https://dummyjson.com/products?limit={DummyProductCount}");

        foreach (var dummyProduct in dummyProducts.Products)
        {
            var categoryId = await GetCategoryIdAsync(dummyProduct.Category.Split("-").First().ToPascalCase());
            
            if (!await _productRepository.AnyAsync(x => x.Name == dummyProduct.Title))
            {
                var image = await DownloadImageAsBytesAsync(dummyProduct.Images?.FirstOrDefault() ?? string.Empty);
                await _productManager.CreateAsync(
                    categoryId,
                    dummyProduct.Title,
                    dummyProduct.Price,
                    dummyProduct.Stock,
                    image,
                    dummyProduct.Description
                );
            }
        }
    }

    private async Task<Guid> GetCategoryIdAsync(string categoryName)
    {
        var categoryId = (await _categoryRepository.FirstOrDefaultAsync(x=> x.Name == categoryName))?.Id;
        
        if (categoryId == null)
        {
            var category = await _categoryManager.CreateAsync(categoryName);

            await _unitOfWorkAccessor.UnitOfWork.SaveChangesAsync();
            
            return category.Id;
        }

        return categoryId.Value;
    }
    
    public async Task<byte[]> DownloadImageAsBytesAsync(string url)
    {
        if (url.IsNullOrWhiteSpace())
        {
            return [];
        }
        
        using var http = new HttpClient();
        await using var stream = await http.GetStreamAsync(url);
        using var image = await Image.LoadAsync(stream);
        using var ms = new MemoryStream();
        await image.SaveAsPngAsync(ms);
        return ms.ToArray();
    }

    private class DummyProductsResponse
    {
        public List<DummyProduct> Products { get; set; }
    }

    private class DummyProduct
    {
        public string Title { get; set; } = "";
        
        public double Price { get; set; }
        
        public int Stock { get; set; }
        
        public string Description { get; set; } = "";
        
        public string Category { get; set; } = "";
        
        public List<string>? Images { get; set; }
    }
}