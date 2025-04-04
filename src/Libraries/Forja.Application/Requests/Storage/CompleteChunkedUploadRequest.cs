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
    public Guid VersionId {get; set;}
    [Required]
    public string FinalFileName {get; set;} = string.Empty;
    public Guid? AddonId {get; set;}
}