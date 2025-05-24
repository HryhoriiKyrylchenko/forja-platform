namespace Forja.Application.Requests.Storage;

public class ProfileHatVariantFileUploadRequest
{
    [Required]
    public short ProfileHatVariantId { get; set; }
    [Required]
    public required IFormFile File { get; set; }
    [Required]
    public long FileSize { get; set; }
    [Required]
    public string ContentType { get; set; } = string.Empty;
    [Required]
    public string FileName { get; set; } = string.Empty;
}