namespace Forja.Application.Requests.Storage;

public class UploadProductImageRequest
{
    [Required]
    public Guid ProductId { get; set; }
    public string ImageAlt { get; set; } = string.Empty;
    [Required]
    public required IFormFile File { get; set; }
    [Required]
    public long ObjectSize { get; set; }
    [Required]
    public string ContentType { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
}