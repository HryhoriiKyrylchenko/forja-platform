namespace Forja.Launcher.Models;

public class StorageMetadata
{
    public string ObjectPath { get; set; } = null!;
    public long Size { get; set; }
    public string ContentType { get; set; } = null!;
    public DateTime? LastModified { get; set; }
}