namespace Forja.Launcher.Models;

public class InstalledFileModel
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string Hash { get; set; } = string.Empty;
    public bool IsArchive { get; set; }
    public FileStatus Status { get; set; } = FileStatus.Unknown;
}