using System;
using Volo.Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;
using Shopularity.Catalog.Products.Admin;
using Volo.Abp;

namespace Shopularity.Catalog.Products;

public class Product : FullAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string Name { get; set; }

    [CanBeNull]
    public virtual string? Description { get; set; }

    public virtual double Price { get; set; }

    public virtual int StockCount { get; set; }
    public Guid? CategoryId { get; set; }

    protected Product()
    {

    }

    public Product(Guid id, Guid? categoryId, string name, double price, int stockCount, string? description = null)
    {

        Id = id;
        Check.NotNull(name, nameof(name));
        Check.Length(name, nameof(name), ProductConsts.NameMaxLength, ProductConsts.NameMinLength);
        Check.Length(description, nameof(description), ProductConsts.DescriptionMaxLength, ProductConsts.DescriptionMinLength);
        Name = name;
        Price = price;
        StockCount = stockCount;
        Description = description;
        CategoryId = categoryId;
    }

}