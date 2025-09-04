using System.ComponentModel.DataAnnotations;
using Shopularity.Catalog.Domain.Categories;

namespace Shopularity.Catalog.Services.Categories.Admin;

public class CategoryCreateDto
{
    [Required]
    [StringLength(CategoryConsts.NameMaxLength, MinimumLength = CategoryConsts.NameMinLength)]
    public string Name { get; set; } = null!;
}