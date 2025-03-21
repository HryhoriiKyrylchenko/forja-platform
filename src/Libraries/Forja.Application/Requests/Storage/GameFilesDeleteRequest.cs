namespace Forja.Application.Requests.Storage;

public class GameFilesDeleteRequest
{
    [Required]
    public string SourcePath { get; set; } = string.Empty;
}