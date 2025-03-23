namespace Forja.Application.Requests.Store;

public class CartCreateRequest
{
    [Required]
    public Guid UserId { get; set; }
}