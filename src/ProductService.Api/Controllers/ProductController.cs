using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Common.CQRS.UseCases.Products.CreateProduct;
using ProductService.Common.CQRS.UseCases.Products.GetListProducts;
using ProductService.Common.CQRS.UseCases.Products.GetProductById;
using ProductService.Common.Dtos.Products;

namespace ProductService.Api.Controllers;


[Route("api/[controller]")]
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
        var query = new GetProductListQuery(model.Request, model.Filter);
        var result = await _sender.Send(query);
        return Ok(result);
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetById([FromQuery] Guid id)
    {
        var query = new GetProductQuery(id);
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
}
