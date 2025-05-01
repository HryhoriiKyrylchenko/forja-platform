namespace Forja.Application.DTOs.Storage;

public class FileMetadataDto
{
    public string ObjectPath { get; set; } = default!;
    public long Size { get; set; }
    public string ContentType { get; set; } = default!;
    public DateTime? LastModified { get; set; }
}