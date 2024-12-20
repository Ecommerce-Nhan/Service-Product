using MediatR;

namespace ProductService.Common.CQRS.UseCases.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest;
