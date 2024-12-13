namespace ProductService.Domain;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Note { get; set; }
    public Guid CreateBy { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public Guid? UpdateBy { get; set; }
    public DateTime? UpdateAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
