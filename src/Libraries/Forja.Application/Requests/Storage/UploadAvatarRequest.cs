namespace Forja.Application.Requests.Storage;

public class UploadAvatarRequest
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public required IFormFile File { get; set; }
    [Required]
    public long ObjectSize { get; set; }
    [Required]
    public string ContentType { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
}