namespace Forja.Application.DTOs.Storage;

public class FileMetadataDto
{
    public string ObjectPath { get; set; } = null!;
    public long Size { get; set; }
    public int Hash { get; set; }
    public string ContentType { get; set; } = null!;
    public DateTime? LastModified { get; set; }
}