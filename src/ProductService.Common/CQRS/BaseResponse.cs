namespace ProductService.Common.CQRS;

public abstract class BaseResponse
{
    public ErrorModel Error { get; set; } = new ErrorModel();
}
