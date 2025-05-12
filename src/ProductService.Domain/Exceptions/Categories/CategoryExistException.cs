using SharedLibrary.Exceptions;
using System.Net;

namespace CategoryService.Domain.Exceptions.Categories;

public class CategoryExistException : BaseException
{
    public CategoryExistException(string code)
        : this(nameof(code), code)
    {
    }
    public CategoryExistException(string name, bool isName = true)
         : this(nameof(name), name)
    {
    }
    private CategoryExistException(string propertyName, string value)
       : base($"Category with {propertyName} '{value}' already taken", HttpStatusCode.Conflict)
    {
    }
}