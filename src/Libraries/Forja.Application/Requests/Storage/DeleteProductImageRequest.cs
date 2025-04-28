namespace Forja.Application.Requests.Storage;

public class DeleteProductImageRequest
{
    [Required]
    public Guid ProductImageId { get; set; }
}