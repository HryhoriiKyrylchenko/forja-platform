namespace Forja.Domain.Entities.Common;

/// <summary>
/// Represents a legal document entity within the system. This class is used to store
/// information related to legal documents, such as their title, content, and effective
/// date. It can optionally include file content in binary format.
/// </summary>
[Table("LegalDocuments", Schema = "common")]
public class LegalDocument
{
    /// <summary>
    /// Gets or sets the unique identifier for a legal document.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the legal document.
    /// This property represents the title or name of the legal document which identifies its content or purpose.
    /// </summary>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the textual or descriptive content associated with a legal document.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the legal document becomes effective.
    /// This property represents the starting point from which the legal document is considered valid or enforceable.
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// Represents the binary content of the document file.
    /// Allows storing the file data related to the legal document, such as PDFs or other document files.
    /// </summary>
    public byte[]? FileContent { get; set; }
}