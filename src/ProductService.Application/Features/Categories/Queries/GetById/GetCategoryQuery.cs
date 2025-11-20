using MediatR;
using SharedLibrary.Dtos.Categories;

namespace ProductService.Application.Features.Categories.Queries.GetById;

public record GetCategoryQuery(Guid Id) : IRequest<CategoryDto>;

