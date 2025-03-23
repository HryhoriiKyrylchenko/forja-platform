namespace Forja.Application.Requests.Store;

public class OrderUpdateRequest
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public OrderStatus Status { get; set; }
}