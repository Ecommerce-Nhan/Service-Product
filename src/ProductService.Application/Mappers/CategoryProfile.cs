using AutoMapper;
using ProductService.Domain.Categories;
using SharedLibrary.Dtos.Categories;

namespace ProductService.Application.Mappers;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<Category, CategoryDto>();
    }
}