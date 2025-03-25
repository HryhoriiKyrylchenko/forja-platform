namespace Forja.Domain.Repositories.Common;

/// <summary>
/// Repository interface for managing LegalDocument data.
/// </summary>
public interface ILegalDocumentRepository
{
    /// <summary>
    /// Retrieves a legal document by its unique identifier.
    /// </summary>
    /// <param name="documentId">The unique identifier of the legal document.</param>
    /// <returns>A legal document if found; otherwise, null.</returns>
    Task<LegalDocument?> GetLegalDocumentByIdAsync(Guid documentId);

    /// <summary>
    /// Retrieves all legal documents with an optional filter by effective date.
    /// </summary>
    /// <param name="effectiveDate">Effective date filter, or null to retrieve all documents.</param>
    /// <returns>A collection of legal documents.</returns>
    Task<IEnumerable<LegalDocument>> GetLegalDocumentsByEffectiveDateAsync(DateTime? effectiveDate = null);

    /// <summary>
    /// Adds a new legal document to the repository.
    /// </summary>
    /// <param name="document">The legal document to add.</param>
    Task<LegalDocument?> AddLegalDocumentAsync(LegalDocument document);

    /// <summary>
    /// Updates an existing legal document.
    /// </summary>
    /// <param name="document">The legal document with updated values.</param>
    Task<LegalDocument?> UpdateLegalDocumentAsync(LegalDocument document);

    /// <summary>
    /// Deletes a legal document by its unique identifier.
    /// </summary>
    /// <param name="documentId">The unique identifier of the legal document.</param>
    Task DeleteLegalDocumentAsync(Guid documentId);
}