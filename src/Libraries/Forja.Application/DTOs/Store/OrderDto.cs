namespace Forja.Application.DTOs.Store;

public class OrderDto
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
}