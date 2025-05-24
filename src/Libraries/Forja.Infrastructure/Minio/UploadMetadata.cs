namespace Forja.Infrastructure.Minio;

public class UploadMetadata
{
    public string UploadId { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public int UploadedChunks { get; set; }
    public int TotalChunks { get; set; }
    public string ContentType { get; set; } = string.Empty;
}