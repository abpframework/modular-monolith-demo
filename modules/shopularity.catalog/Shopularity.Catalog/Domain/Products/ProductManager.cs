using System;
using System.Threading.Tasks;
using Shopularity.Catalog.Services.Products.Admin;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace Shopularity.Catalog.Domain.Products;

public class ProductManager : DomainService
{
    protected IProductRepository _productRepository;
    private readonly IBlobContainer _blobContainer;

    public ProductManager(
        IProductRepository productRepository,
        IBlobContainer blobContainer)
    {
        _productRepository = productRepository;
        _blobContainer = blobContainer;
    }

    public virtual async Task<Product> CreateAsync(
        Guid? categoryId,
        string name,
        double price,
        int stockCount,
        byte[]? image = null,
        string? description = null)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Check.Length(name, nameof(name), ProductConsts.NameMaxLength);
        Check.Length(description, nameof(description), ProductConsts.DescriptionMaxLength);

        var product = new Product(
            GuidGenerator.Create(),
            categoryId,
            name,
            price,
            stockCount,
            description);

        if (image != null && image.Length != 0)
        {
            await _blobContainer.SaveAsync(product.Id.ToString(), image, overrideExisting: true);
        }

        return await _productRepository.InsertAsync(product);
    }

    public virtual async Task<Product> UpdateAsync(
        Guid id,
        Guid? categoryId,
        string name,
        double price,
        int stockCount,
        byte[]? image = null,
        string? description = null,
        string? concurrencyStamp = null)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Check.Length(name, nameof(name), ProductConsts.NameMaxLength);
        Check.Length(description, nameof(description), ProductConsts.DescriptionMaxLength);

        var product = await _productRepository.GetAsync(id);

        product.CategoryId = categoryId;
        product.Name = name;
        product.Price = price;
        product.StockCount = stockCount;
        product.Description = description;

        product.SetConcurrencyStampIfNotNull(concurrencyStamp);
        var savedProduct = await _productRepository.UpdateAsync(product);

        if (image != null && image.Length != 0)
        {
            await _blobContainer.SaveAsync(product.Id.ToString(), image, overrideExisting: true);
        }
        else
        {
            await _blobContainer.DeleteAsync(product.Id.ToString());
        }

        return savedProduct;
    }
    
    public async Task DecreaseCountAsync(Guid id, int amount)
    {
        var product = await _productRepository.GetAsync(id);
        product.StockCount -= amount;
        await _productRepository.UpdateAsync(product);
    }
}