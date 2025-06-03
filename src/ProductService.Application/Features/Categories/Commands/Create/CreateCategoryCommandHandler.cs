using CategoryService.Domain.Categories;
using MediatR;

namespace CategoryService.Application.Features.Categories.Commands.Create;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
    private readonly ICategoryRepository _repository;
    private readonly CategoryManager _manager;
    public CreateCategoryCommandHandler(ICategoryRepository repository,
                                        CategoryManager manager)
    {
        _repository = repository;
        _manager = manager;
    }
    public async Task<Guid> Handle(CreateCategoryCommand command,
                                   CancellationToken cancellationToken)
    {
        var input = command.Model;
        var category = await _manager.CreateAsync(input.Name,
                                                  input.Code,
                                                  input.Description);

        await _repository.AddAsync(category);
        return category.Id;
    }
}