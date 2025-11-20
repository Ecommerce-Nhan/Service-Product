using AutoMapper;
using CategoryService.Domain.Exceptions.Categories;
using CategoryService.Domain.Categories;
using MediatR;
using ProductService.Domain.Categories;
using SharedLibrary.Dtos.Categories;

namespace ProductService.Application.Features.Categories.Queries.GetById;

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDto>
{
    private readonly IMapper _mapper;
    private readonly ICategoryReadOnlyRepository _repository;

    public GetCategoryQueryHandler(IMapper mapper, ICategoryReadOnlyRepository repository)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<CategoryDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id);
        if (category == null)
        {
            throw new CategoryNotFoundException(request.Id);
        }
        var result = _mapper.Map<CategoryDto>(category);
        return result;
    }
}

