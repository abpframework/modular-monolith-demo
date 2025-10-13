using System.ComponentModel.DataAnnotations;
using Shopularity.Catalog.Domain.Categories;
using Volo.Abp.Domain.Entities;

namespace Shopularity.Catalog.Services.Categories.Admin;

public class CategoryUpdateDto : IHasConcurrencyStamp
{
    [Required]
    [StringLength(CategoryConsts.NameMaxLength, MinimumLength = CategoryConsts.NameMinLength)]
    public string Name { get; set; } = null!;

    public string ConcurrencyStamp { get; set; } = null!;
}