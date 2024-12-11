using AutoMapper;
using ProductService.Common.Dtos.Products;
using ProductService.Domain.Products;

namespace ProductService.Application.Mappers;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductDto, Product>();
    }
}
