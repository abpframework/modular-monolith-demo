using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace Shopularity.Catalog.Products
{
    public class ProductManager : DomainService
    {
        protected IProductRepository _productRepository;

        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public virtual async Task<Product> CreateAsync(
        Guid? categoryId, string name, double price, int stockCount, string? description = null)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.Length(name, nameof(name), ProductConsts.NameMaxLength, ProductConsts.NameMinLength);
            Check.Length(description, nameof(description), ProductConsts.DescriptionMaxLength, ProductConsts.DescriptionMinLength);

            var product = new Product(
             GuidGenerator.Create(),
             categoryId, name, price, stockCount, description
             );

            return await _productRepository.InsertAsync(product);
        }

        public virtual async Task<Product> UpdateAsync(
            Guid id,
            Guid? categoryId, string name, double price, int stockCount, string? description = null, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.Length(name, nameof(name), ProductConsts.NameMaxLength, ProductConsts.NameMinLength);
            Check.Length(description, nameof(description), ProductConsts.DescriptionMaxLength, ProductConsts.DescriptionMinLength);

            var product = await _productRepository.GetAsync(id);

            product.CategoryId = categoryId;
            product.Name = name;
            product.Price = price;
            product.StockCount = stockCount;
            product.Description = description;

            product.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _productRepository.UpdateAsync(product);
        }

    }
}