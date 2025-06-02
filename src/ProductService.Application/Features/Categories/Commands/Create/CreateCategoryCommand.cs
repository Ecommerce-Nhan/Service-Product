using MediatR;
using SharedLibrary.Dtos.Categories;

namespace CategoryService.Application.Features.Categories.Commands.Create;

public record CreateCategoryCommand(CreateCategoryDto Model) : IRequest<Guid>;