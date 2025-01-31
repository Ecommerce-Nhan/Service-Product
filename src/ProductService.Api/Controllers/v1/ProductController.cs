using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Features.Products.Commands.Create;
using ProductService.Application.Features.Products.Commands.Delete;
using ProductService.Application.Features.Products.Commands.Update;
using ProductService.Application.Features.Products.Queries.GetById;
using ProductService.Application.Features.Products.Queries.GetList;
using SharedLibrary.Dtos.Products;

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

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ViewProductPermission")]
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll([FromQuery] ListProductsQuery model)
    {
        var query = new ListProductsQuery(model.Sort, model.Filter);
        var result = await _sender.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] CreateProductDto model)
    {
        var command = new CreateProductCommand(model);
        var result = await _sender.Send(command);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromForm] UpdateProductDto model)
    {
        var command = new UpdateProductCommand(model);
        await _sender.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
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
