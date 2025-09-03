using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace Shopularity.Catalog.Categories;

public class CategoryManager : DomainService
{
    protected ICategoryRepository _categoryRepository;

    public CategoryManager(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public virtual async Task<Category> CreateAsync(
        string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Check.Length(name, nameof(name), CategoryConsts.NameMaxLength, CategoryConsts.NameMinLength);

        //TODO: Check duplicate name!
        
        var category = new Category(
            GuidGenerator.Create(),
            name
        );

        return await _categoryRepository.InsertAsync(category);
    }

    public virtual async Task<Category> UpdateAsync(
        Guid id,
        string name, [CanBeNull] string? concurrencyStamp = null
    )
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Check.Length(name, nameof(name), CategoryConsts.NameMaxLength, CategoryConsts.NameMinLength);

        var category = await _categoryRepository.GetAsync(id);
        
        //TODO: Check duplicate name!

        category.Name = name;

        category.SetConcurrencyStampIfNotNull(concurrencyStamp); //TODO: Necessary?
        return await _categoryRepository.UpdateAsync(category);
    }
}