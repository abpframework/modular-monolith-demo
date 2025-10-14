using System;
using JetBrains.Annotations;
using Shopularity.Catalog.Services.Categories.Admin;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Shopularity.Catalog.Domain.Categories;

public class Category : FullAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string Name { get; set; }

    protected Category()
    {

    }

    public Category(Guid id, string name)
    {
        Id = id;
        Check.NotNull(name, nameof(name));
        Check.Length(name, nameof(name), CategoryConsts.NameMaxLength, CategoryConsts.NameMinLength);
        Name = name;
    }
}