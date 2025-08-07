using System.ComponentModel.DataAnnotations;

namespace Shopularity.Catalog.Categories;

public class CategoryCreateDto
{
    [Required]
    [StringLength(CategoryConsts.NameMaxLength, MinimumLength = CategoryConsts.NameMinLength)]
    public string Name { get; set; } = null!;
}