namespace Forja.Application.Requests.Storage;

public class DeleteObjectRequest
{
    [Required]
    public string ObjectPath { get; set; } = string.Empty;
}