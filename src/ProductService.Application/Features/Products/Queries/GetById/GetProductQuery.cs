using MediatR;
using SharedLibrary.Dtos.Products;

namespace ProductService.Application.Features.Products.Queries.GetById;

public record GetProductQuery(Guid Id) : IRequest<ProductDto>;
