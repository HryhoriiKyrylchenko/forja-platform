namespace Forja.Application.Requests.Storage;

public class CompleteChunkedUploadRequest
{
    [Required]
    public string UploadId {get; set;} = string.Empty;
    [Required]
    public FileType FileType {get; set;}
    [Required]
    public Guid GameId {get; set;}
    [Required]
    public string Version {get; set;} = string.Empty;
    [Required]
    public string FinalFileName {get; set;} = string.Empty;
    public string ChangeLog {get; set;} = string.Empty;
    public DateTime? ReleaseDate {get; set;}
    public string? FromVersion { get; set; } = string.Empty;
    public string? ToVersion { get; set; } = string.Empty;
    public Guid? AddonId {get; set;}
}