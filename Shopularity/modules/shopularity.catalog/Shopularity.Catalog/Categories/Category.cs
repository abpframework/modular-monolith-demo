using System;
using Volo.Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;

using Volo.Abp;

namespace Shopularity.Catalog.Categories
{
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
}