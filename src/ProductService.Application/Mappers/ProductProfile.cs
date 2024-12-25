using AutoMapper;
using SharedLibrary.Dtos.Products;
using ProductService.Domain.Products;

namespace ProductService.Application.Mappers;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductDto, Product>();
        CreateMap<Product, ProductDto>();
    }
}
