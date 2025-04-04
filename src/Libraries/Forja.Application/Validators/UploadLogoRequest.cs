namespace Forja.Application.Validators;

public class UploadLogoRequest
{
    [Required]
    public Guid ProductId { get; set; }
    [Required]
    public required IFormFile File { get; set; }
    [Required]
    public long ObjectSize { get; set; }
    [Required]
    public string ContentType { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
}