namespace ProductService.Common.CQRS.Models.Responses;

public abstract class BaseResponse
{
    public ErrorModel Error { get; set; } = new ErrorModel();
}
