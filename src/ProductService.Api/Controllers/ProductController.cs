using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Common.CQRS.UseCases.Products.CreateProduct;
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

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Guid id)
    {
        var result = await _sender.Send(new GetProductQuery(id));
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
