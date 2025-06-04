using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductService.Api.Controllers.v1;
using ProductService.Application.Features.Products.Queries.GetById;
using SharedLibrary.Dtos.Products;

namespace ProductService.Api.Test.Controllers;

public class ProductControllerTest
{
    private readonly ProductController _controller;
    private readonly Mock<ISender> _mockSender;
    public ProductControllerTest()
    {
        _mockSender = new Mock<ISender>();
        _controller = new ProductController(_mockSender.Object);
    }
    [Fact]
    public async Task GetById_ReturnsOkResult_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var mockProduct = new ProductDto(productId, categoryId, "Test Product", "105065", "note", 10000, new List<string>());
        _mockSender.Setup(sender => sender.Send(It.IsAny<GetProductQuery>(), default))
            .ReturnsAsync(mockProduct);

        // Act
        var result = await _controller.GetById(productId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ProductDto>(okResult.Value);
        Assert.Equal(mockProduct.Id, returnValue.Id);
        Assert.Equal(mockProduct.Name, returnValue.Name);
    }
    [Fact]
    public async Task GetById_ReturnsNotFoundResult_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();
        //_mockSender.Setup(sender => sender.Send(It.IsAny<GetProductQuery>(), default))
        //    .ReturnsAsync((ProductDto?)null);

        // Act
        var result = await _controller.GetById(productId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}