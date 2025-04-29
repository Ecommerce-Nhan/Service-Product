using AutoMapper;
using ProductService.Domain.Products;
using SharedLibrary.Dtos.Products;

namespace ProductService.Application.Mappers;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductDto, Product>();
        CreateMap<Product, ProductDto>();
    }
}
