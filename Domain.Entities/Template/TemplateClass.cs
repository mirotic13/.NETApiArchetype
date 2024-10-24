namespace Domain.Entities.Template;
public class TemplateClass
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public TemplateEnum Status { get; set; }

    public decimal Amount { get; set; }
}

public enum TemplateEnum
{
    Pending,
    Completed,
    Cancelled
}
