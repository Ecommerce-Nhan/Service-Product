using CategoryService.Domain.Categories;
using CategoryService.Domain.Exceptions.Categories;
using MediatR;
using ProductService.Domain.Categories;

namespace ProductService.Application.Features.Categories.Commands.Update;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
{
    private readonly ICategoryRepository _repository;
    private readonly CategoryManager _manager;
    public UpdateCategoryCommandHandler(ICategoryRepository repository,
                                        CategoryManager manager)
    {
        _repository = repository;
        _manager = manager;
    }
    public async Task Handle(UpdateCategoryCommand command,
                             CancellationToken cancellationToken)
    {
        var input = command.Model;
        var category = _repository.GetQueryable().FirstOrDefault(x => x.Id == command.Id);
        if (category == null)
        {
            throw new CategoryNotFoundException(command.Id);
        }
        if (category.Code != input.Code)
        {
            await _manager.ChangeCodeAsync(category, input.Code);
        }

        if (category.Name != input.Name)
        {
            await _manager.ChangeNameAsync(category, input.Name);
        }
        category.Description = input.Description;

        await _repository.Update(category);
    }
}

