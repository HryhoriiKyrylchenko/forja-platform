namespace Forja.Application.Requests.Common;

public class LegalDocumentUpdateRequest
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Content { get; set; } = string.Empty;
    public DateTime EffectiveDate { get; set; }
    public byte[]? FileContent { get; set; }
}