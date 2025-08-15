using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Shopularity.Catalog.Products;

public interface IProductRepository : IRepository<Product, Guid>
{

    Task DeleteAllAsync(
        string? filterText = null,
        string? name = null,
        string? description = null,
        double? priceMin = null,
        double? priceMax = null,
        int? stockCountMin = null,
        int? stockCountMax = null,
        Guid? categoryId = null,
        CancellationToken cancellationToken = default);
    Task<ProductWithNavigationProperties> GetWithNavigationPropertiesAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    Task<List<ProductWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
        string? filterText = null,
        string? name = null,
        string? description = null,
        double? priceMin = null,
        double? priceMax = null,
        int? stockCountMin = null,
        int? stockCountMax = null,
        Guid? categoryId = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<List<ProductWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
        List<Guid> ids,
        CancellationToken cancellationToken = default
    );

    Task<List<Product>> GetListAsync(
        string? filterText = null,
        string? name = null,
        string? description = null,
        double? priceMin = null,
        double? priceMax = null,
        int? stockCountMin = null,
        int? stockCountMax = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<List<Product>> GetListAsync(
        List<Guid> ids,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        string? filterText = null,
        string? name = null,
        string? description = null,
        double? priceMin = null,
        double? priceMax = null,
        int? stockCountMin = null,
        int? stockCountMax = null,
        Guid? categoryId = null,
        CancellationToken cancellationToken = default);
}