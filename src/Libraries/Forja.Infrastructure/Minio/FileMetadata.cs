namespace Forja.Infrastructure.Minio;

public class FileMetadata
{
    public string ObjectPath { get; set; } = default!;
    public long Size { get; set; }
    public string ContentType { get; set; } = default!;
    public DateTime? LastModified { get; set; }
}