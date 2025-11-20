using MediatR;
using SharedLibrary.Dtos.Categories;

namespace ProductService.Application.Features.Categories.Commands.Create;

public record CreateCategoryCommand(CreateCategoryDto Model) : IRequest<Guid>;