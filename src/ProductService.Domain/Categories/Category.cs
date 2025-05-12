namespace ProductService.Domain.Categories;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }

    internal Category ChangeCode(string code)
    {
        SetCode(code);
        return this;
    }
    internal Category ChangeName(string name)
    {
        SetName(name);
        return this;
    }
    private void SetCode(string code)
    {
        Code = code;
    }
    private void SetName(string name)
    {
        Name = name;
    }
}