using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Common.CQRS.UseCases.Products.CreateProduct;
using ProductService.Common.CQRS.UseCases.Products.DeleteProduct;
using ProductService.Common.CQRS.UseCases.Products.GetListProducts;
using ProductService.Common.CQRS.UseCases.Products.GetProductById;
using ProductService.Common.CQRS.UseCases.Products.UpdateProduct;
using ProductService.Common.Dtos.Products;

namespace ProductService.Api.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ISender _sender;
    public ProductController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll([FromQuery] GetProductListQuery model)
    {
        var query = new GetProductListQuery(model.Sort, model.Filter);
        var result = await _sender.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateProductDto model)
    {
        var command = new CreateProductCommand(model);
        var result = await _sender.Send(command);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UpdateProductDto model)
    {
        var command = new UpdateProductCommand(model);
        await _sender.Send(command);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        var command = new DeleteProductCommand(id);
        await _sender.Send(command);
        return NoContent();
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetById([FromQuery] Guid id)
    {
        var query = new GetProductQuery(id);
        var result = await _sender.Send(query);
        return Ok(result);
    }
}
