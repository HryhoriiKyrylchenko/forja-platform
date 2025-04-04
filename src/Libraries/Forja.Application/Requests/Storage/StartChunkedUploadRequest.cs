namespace Forja.Application.Requests.Storage;

public class StartChunkedUploadRequest
{
    [Required]
    public string FileName { get; set; } = string.Empty;
    [Required]
    public long FileSize { get; set; }
    [Required]
    public int TotalChunks { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public string ContentType { get; set; } = string.Empty;
}