namespace Forja.Application.DTOs.Support;

public class FAQDto
{
    public Guid Id { get; set; }
    public string Question { get; set; } = String.Empty;
    public string Answer { get; set; } = String.Empty;
    public int Order { get; set; } = 0;
}