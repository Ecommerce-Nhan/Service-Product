namespace ProductService.Domain.Products;

public class Product : BaseEntity
{
    public float CostPrice { get; set; }
    public float UnitPrice { get; set; }
    public List<string>? Images { get; set; }

    internal Product ChangeCode(string code)
    {
        SetCode(code);
        return this;
    }
    internal Product ChangeName(string name)
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
