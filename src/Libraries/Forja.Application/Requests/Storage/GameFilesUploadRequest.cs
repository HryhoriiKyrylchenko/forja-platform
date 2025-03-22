namespace Forja.Application.Requests.Storage;

public class GameFilesUploadRequest
{
    [Required]
    public string FolderPath { get; set; } = string.Empty;
}