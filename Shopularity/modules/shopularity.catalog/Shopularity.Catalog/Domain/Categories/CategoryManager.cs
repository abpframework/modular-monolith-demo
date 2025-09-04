using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Shopularity.Catalog.Services.Categories.Admin;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Shopularity.Catalog.Domain.Categories;

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
        Check.Length(name, nameof(name), CategoryConsts.NameMaxLength);

        await CheckDuplicateNameAsync(name);
        
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
        Check.Length(name, nameof(name), CategoryConsts.NameMaxLength);

        await CheckDuplicateNameAsync(name, id);

        var category = await _categoryRepository.GetAsync(id);

        category.Name = name;

        category.SetConcurrencyStampIfNotNull(concurrencyStamp); //TODO: Necessary?
        return await _categoryRepository.UpdateAsync(category);
    }

    private async Task CheckDuplicateNameAsync(string name, Guid? id = null)
    {
        var existingCategory = await _categoryRepository.FirstOrDefaultAsync(x=> x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        if (existingCategory == null)
        {
            return;
        }

        if (existingCategory.Id == id)
        {
            return;
        }
        
        throw new BusinessException(CatalogErrorCodes.CategoryNameAlreadyExists)
            .WithData("Name", name);
    }
}