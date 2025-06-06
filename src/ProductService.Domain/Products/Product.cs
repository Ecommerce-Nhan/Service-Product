﻿namespace ProductService.Domain.Products;

public class Product : BaseEntity
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Note { get; set; }
    public decimal CostPrice { get; set; }
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