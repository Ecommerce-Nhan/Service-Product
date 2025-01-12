using MediatR;
using SharedLibrary.Dtos.Products;

namespace ProductService.Application.Features.Products.Commands.Update;

public record UpdateProductCommand(UpdateProductDto Model) : IRequest;
