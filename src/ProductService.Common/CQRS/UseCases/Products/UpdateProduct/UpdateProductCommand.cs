using MediatR;
using ProductService.Common.Dtos.Products;

namespace ProductService.Common.CQRS.UseCases.Products.UpdateProduct;

public record UpdateProductCommand(UpdateProductDto Model) : IRequest;