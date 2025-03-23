namespace Forja.Application.Requests.Store;

public class OrderCreateRequest
{
    [Required]
    public Guid CartId { get; set; }
}