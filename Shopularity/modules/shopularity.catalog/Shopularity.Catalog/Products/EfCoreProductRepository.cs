using Shopularity.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Shopularity.Catalog.Data;
using Shopularity.Catalog.Products.Admin;

namespace Shopularity.Catalog.Products;

public class EfCoreProductRepository : EfCoreRepository<CatalogDbContext, Product, Guid>, IProductRepository
{
    public EfCoreProductRepository(IDbContextProvider<CatalogDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task DeleteAllAsync(
        string? filterText = null,
        string? name = null,
        string? description = null,
        double? priceMin = null,
        double? priceMax = null,
        int? stockCountMin = null,
        int? stockCountMax = null,
        Guid? categoryId = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();

        query = ApplyFilter(query, filterText, name, description, priceMin, priceMax, stockCountMin, stockCountMax, categoryId);

        var ids = query.Select(x => x.Product.Id);
        await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<ProductWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        return (await GetDbSetAsync()).Where(b => b.Id == id)
            .Select(product => new ProductWithNavigationProperties
            {
                Product = product,
                Category = dbContext.Set<Category>().FirstOrDefault(c => c.Id == product.CategoryId)
            }).FirstOrDefault();
    }

    public virtual async Task<List<ProductWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
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
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, name, description, priceMin, priceMax, stockCountMin, stockCountMax, categoryId);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ProductConsts.GetDefaultSorting(true) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<ProductWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
        List<Guid> ids,
        CancellationToken cancellationToken = default
    )
    {
        if (ids.Count == 0)
        {
            return new List<ProductWithNavigationProperties>();
        }
        
        var query = await GetQueryForNavigationPropertiesAsync();
        query = query.Where(x=> ids.Contains(x.Product.Id));
        return await query.ToListAsync(cancellationToken);
    }

    protected virtual async Task<IQueryable<ProductWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
    {
        return from product in (await GetDbSetAsync())
            join category in (await GetDbContextAsync()).Set<Category>() on product.CategoryId equals category.Id into categories
            from category in categories.DefaultIfEmpty()
            select new ProductWithNavigationProperties
            {
                Product = product,
                Category = category
            };
    }

    protected virtual IQueryable<ProductWithNavigationProperties> ApplyFilter(
        IQueryable<ProductWithNavigationProperties> query,
        string? filterText,
        string? name = null,
        string? description = null,
        double? priceMin = null,
        double? priceMax = null,
        int? stockCountMin = null,
        int? stockCountMax = null,
        Guid? categoryId = null)
    {
        return query
            .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Product.Name!.Contains(filterText!) || e.Product.Description!.Contains(filterText!))
            .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.Product.Name.Contains(name))
            .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Product.Description.Contains(description))
            .WhereIf(priceMin.HasValue, e => e.Product.Price >= priceMin!.Value)
            .WhereIf(priceMax.HasValue, e => e.Product.Price <= priceMax!.Value)
            .WhereIf(stockCountMin.HasValue, e => e.Product.StockCount >= stockCountMin!.Value)
            .WhereIf(stockCountMax.HasValue, e => e.Product.StockCount <= stockCountMax!.Value)
            .WhereIf(categoryId != null && categoryId != Guid.Empty, e => e.Category != null && e.Category.Id == categoryId);
    }

    public virtual async Task<List<Product>> GetListAsync(
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
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, name, description, priceMin, priceMax, stockCountMin, stockCountMax);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ProductConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<Product>> GetListAsync(
        List<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryableAsync()).Where(x=> ids.Contains(x.Id));
        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        string? filterText = null,
        string? name = null,
        string? description = null,
        double? priceMin = null,
        double? priceMax = null,
        int? stockCountMin = null,
        int? stockCountMax = null,
        Guid? categoryId = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, name, description, priceMin, priceMax, stockCountMin, stockCountMax, categoryId);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<Product> ApplyFilter(
        IQueryable<Product> query,
        string? filterText = null,
        string? name = null,
        string? description = null,
        double? priceMin = null,
        double? priceMax = null,
        int? stockCountMin = null,
        int? stockCountMax = null)
    {
        return query
            .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Name!.Contains(filterText!) || e.Description!.Contains(filterText!))
            .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.Name.Contains(name))
            .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Description.Contains(description))
            .WhereIf(priceMin.HasValue, e => e.Price >= priceMin!.Value)
            .WhereIf(priceMax.HasValue, e => e.Price <= priceMax!.Value)
            .WhereIf(stockCountMin.HasValue, e => e.StockCount >= stockCountMin!.Value)
            .WhereIf(stockCountMax.HasValue, e => e.StockCount <= stockCountMax!.Value);
    }
}