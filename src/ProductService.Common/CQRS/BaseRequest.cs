namespace ProductService.Common.CQRS;

public abstract class BaseRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Note { get; set; }
    public Guid CreateBy { get; set; }
    public DateTime CreateAt { get; set; }
    public Guid? UpdateBy { get; set; }
    public DateTime? UpdateAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
