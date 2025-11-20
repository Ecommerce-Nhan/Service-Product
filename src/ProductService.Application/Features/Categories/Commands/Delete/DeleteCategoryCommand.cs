using MediatR;

namespace ProductService.Application.Features.Categories.Commands.Delete;

public record DeleteCategoryCommand(Guid Id) : IRequest;

