namespace ProductService.Common.CQRS;

public class ErrorModel
{
    public List<string> Errors = new List<string>();
    public bool IsEmpty
    {
        get
        {
            return !Errors.Any();
        }
    }

    public void Add(string error)
    {
        Errors.Add(error);
    }
}
