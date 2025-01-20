using MediatR;
using ProductService.Domain.Products;
using SharedLibrary.Dtos.Products;

namespace ProductService.Application.Features.Products.Notifications;

public record ProductCreatedNotification(CreateProductDto model, Product entity) : INotification;