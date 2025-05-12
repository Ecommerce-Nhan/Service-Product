using CategoryService.Domain.Exceptions.Categories;
using ProductService.Domain.Categories;
using SharedLibrary.Exceptions;

namespace CategoryService.Domain.Categories;

public class CategoryManager
{
    private readonly ICategoryReadOnlyRepository _repository;
    public CategoryManager(ICategoryReadOnlyRepository repository)
    {
        _repository = repository;
    }
    public async Task<Category> CreateAsync(string name,
                         string code,
                         string? description)
    //List<string>? images)
    {
        var existingEntity = await _repository.FindByCodeAsync(code);
        if (existingEntity != null)
        {
            throw new CategoryExistException(code);
        }
        existingEntity = await _repository.FindByNameAsync(name);
        if (existingEntity != null)
        {
            throw new CategoryExistException(name, true);
        }

        return new Category
        {
            Name = name,
            Code = code,
            Description = description
        };
    }

    public async Task ChangeCodeAsync(Category product, string newCode)
    {
        if (string.IsNullOrEmpty(newCode) || string.IsNullOrWhiteSpace(newCode))
        {
            throw new BaseException("Code is required");
        }

        var existingEntity = await _repository.FindByCodeAsync(newCode);
        if (existingEntity != null && existingEntity.Id != product.Id)
        {
            throw new CategoryExistException(newCode, false);
        }

        product.ChangeCode(newCode);
    }

    public async Task ChangeNameAsync(Category product, string newName)
    {
        if (string.IsNullOrEmpty(newName) || string.IsNullOrWhiteSpace(newName))
        {
            throw new BaseException("Name is required");
        }

        var existingEntity = await _repository.FindByNameAsync(newName);
        if (existingEntity != null && existingEntity.Id != product.Id)
        {
            throw new CategoryExistException(newName, true);
        }

        product.ChangeName(newName);
    }
}
