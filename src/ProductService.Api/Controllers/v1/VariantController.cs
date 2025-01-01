using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.CQRS.UseCases.Variants.GetVariantByProductId;

namespace ProductService.Api.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class VariantController : ControllerBase
{
    private readonly ISender _sender;
    public VariantController(ISender sender)
    {
        _sender = sender;
    }
    [HttpGet("GetVariantListByProductId")]
    public async Task<IActionResult> GetVariantListByProductId([FromQuery] GetVariantListQuery model)
    {
        var query = new GetVariantListQuery(model.ProductId, model.Filter);
        var result = await _sender.Send(query);
        return Ok(result);
    }
}
