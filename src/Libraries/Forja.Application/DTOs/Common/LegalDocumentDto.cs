namespace Forja.Application.DTOs.Common;

public class LegalDocumentDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime EffectiveDate { get; set; }
    public byte[]? FileContent { get; set; }
}