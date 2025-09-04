using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Shopularity.Catalog.Services.Categories.Admin;

public class CategoryDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string Name { get; set; } = null!;

    public string ConcurrencyStamp { get; set; } = null!;

}