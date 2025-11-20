using CategoryService.Domain.Categories;
using CategoryService.Domain.Exceptions.Categories;
using MediatR;
using ProductService.Domain.Categories;

namespace ProductService.Application.Features.Categories.Commands.Delete;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly ICategoryRepository _repository;
    public DeleteCategoryCommandHandler(ICategoryRepository repository)
    {
        _repository = repository;
    }
    public async Task Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = _repository.GetQueryable().FirstOrDefault(x => x.Id == command.Id);
        if (category == null)
        {
            throw new CategoryNotFoundException(command.Id);
        }
        await _repository.Remove(category);
    }
}

