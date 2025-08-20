using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Internal.Mappers;
using JetBrains.Annotations;
using Shopularity.Catalog.Products.Admin;
using Shopularity.Catalog.Products.Events;
using Shopularity.Catalog.Products.Public;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using IObjectMapper = Volo.Abp.ObjectMapping.IObjectMapper;

namespace Shopularity.Catalog.Products;

public class ProductManager : DomainService
{
    protected IProductRepository _productRepository;
    private readonly IBlobContainer _blobContainer;
    private readonly IObjectMapper _objectMapper;
    private readonly IDistributedEventBus _eventBus;

    public ProductManager(
        IProductRepository productRepository,
        IBlobContainer blobContainer,
        IObjectMapper objectMapper,
        IDistributedEventBus eventBus)
    {
        _productRepository = productRepository;
        _blobContainer = blobContainer;
        _objectMapper = objectMapper;
        _eventBus = eventBus;
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
    
    public async Task RequestProductsAsync(ProductsRequestedInput input)
    {
        var products = await _productRepository.GetListAsync(input.Products.Select(x=> x.Key).ToList());

        foreach (var product in products)
        {
            var amount = input.Products[product.Id];

            if (amount > product.StockCount)
            {
                //todo: business exception
                throw new UserFriendlyException($"Not Enough Stock!! {amount} > {product.StockCount}");
            }

            product.StockCount -= amount;

            await _productRepository.UpdateAsync(product);
        }
        
        var productsAsDto = _objectMapper.Map<List<Product>, List<ProductDto>>(products);

        await _eventBus.PublishAsync(new ProductsRequestCompletedEto
        {
            Products = productsAsDto.Select(x=>
                new KeyValuePair<ProductDto, int>(x, input.Products[x.Id])
            ).ToList(),
            RequesterId = input.RequesterId
        });
    }
}