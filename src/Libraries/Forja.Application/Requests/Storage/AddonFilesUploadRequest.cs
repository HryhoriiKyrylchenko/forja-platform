namespace Forja.Application.Requests.Storage;

public class AddonFilesUploadRequest
{
    [Required]
    public string FolderPath { get; set; } = string.Empty;
}