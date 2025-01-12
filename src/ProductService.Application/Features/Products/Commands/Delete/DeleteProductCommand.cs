using MediatR;

namespace ProductService.Application.Features.Products.Commands.Delete;

public record DeleteProductCommand(Guid Id) : IRequest;
