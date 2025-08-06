using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Shopularity.Catalog.Categories
{
    public class CategoryCreateDto
    {
        [Required]
        [StringLength(CategoryConsts.NameMaxLength, MinimumLength = CategoryConsts.NameMinLength)]
        public string Name { get; set; } = null!;
    }
}