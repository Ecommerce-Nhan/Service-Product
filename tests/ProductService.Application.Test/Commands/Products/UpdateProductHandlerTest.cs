using Microsoft.AspNetCore.Http;
using NSubstitute;
using ProductService.Application.Features.Products.Commands.Update;
using ProductService.Domain.Exceptions.Products;
using ProductService.Domain.Products;
using SharedLibrary.Dtos.Products;

namespace ProductService.Application.Test.Commands.Products;

public class UpdateProductHandlerTest
{
    private readonly IProductRepository _repositoryMock;
    private readonly IProductReadOnlyRepository _readOnlyRepositoryMock;
    private readonly ProductManager _managerMock;
    private readonly UpdateProductCommandHandler _handler;

    private static readonly Guid productID = Guid.NewGuid();
    private static readonly UpdateProductDto input
        = new UpdateProductDto(productID, "name", "code", null, 10, new List<IFormFile>());

    public UpdateProductHandlerTest()
    {
        _repositoryMock = Substitute.For<IProductRepository>();
        _readOnlyRepositoryMock = Substitute.For<IProductReadOnlyRepository>();
        _managerMock = Substitute.For<ProductManager>(_readOnlyRepositoryMock);
        _handler = new UpdateProductCommandHandler(_repositoryMock, _managerMock);
    }

    [Fact]
    public async Task Handle_ProductNotFound_ThrowsProductNotFoundException()
    {
        // Arrange
        var command = new UpdateProductCommand(input);
        _repositoryMock.GetQueryable().Returns(Enumerable.Empty<Product>().AsQueryable());

        // Act & Assert
        await Assert.ThrowsAsync<ProductNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ProductNameChanged_CallsChangeNameAsync()
    {
        // Arrange
        var input = new UpdateProductDto(productID, "NewName", "code", null, 10, new List<IFormFile>());
        var command = new UpdateProductCommand(input);
        var product = new Product { Id = productID, Name = "OldName" };

        _repositoryMock.GetQueryable().Returns(new[] { product }.AsQueryable());
        _readOnlyRepositoryMock.GetQueryable().Returns(new[] { product }.AsQueryable());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _managerMock.Received(1).ChangeNameAsync(
             Arg.Is<Product>(p => p.Name == "NewName" && p.Id == productID),
             Arg.Is<string>("NewName")
        );
    }

    [Fact]
    public async Task Handle_ProductUpdated_CallsRepositoryUpdate()
    {
        // Arrange
        var product = new Product { Id = productID, Name = "HandleProductUpdate" };
        var command = new UpdateProductCommand(input);
        _repositoryMock.GetQueryable().Returns(new[] { product }.AsQueryable());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _repositoryMock.Received(1).Update(product);
        Assert.Equal("name", product.Name);
    }
}
