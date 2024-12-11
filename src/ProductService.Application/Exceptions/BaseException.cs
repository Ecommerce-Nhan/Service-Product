using System.Net;

namespace ProductService.Application.Exceptions;

public class BaseException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public BaseException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        : base(message)
    {
        StatusCode = statusCode;
    }
}
