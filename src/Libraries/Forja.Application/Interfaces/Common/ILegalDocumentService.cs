namespace Forja.Application.Interfaces.Common;

/// <summary>
/// Service interface for handling operations related to LegalDocuments.
/// </summary>
public interface ILegalDocumentService
{
    /// <summary>
    /// Retrieves a legal document by its unique identifier.
    /// </summary>
    /// <param name="documentId">The unique identifier of the legal document to retrieve.</param>
    /// <returns>A <see cref="LegalDocumentDto"/> representing the legal document if found; otherwise, null.</returns>
    Task<LegalDocumentDto?> GetLegalDocumentByIdAsync(Guid documentId);

    /// <summary>
    /// Retrieves legal documents, optionally filtered by effective date.
    /// </summary>
    /// <param name="effectiveDate">
    /// The effective date filter. If null, all legal documents will be retrieved.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation that returns a collection of legal documents as DTOs.
    /// </returns>
    Task<IEnumerable<LegalDocumentDto>> GetLegalDocumentsByEffectiveDateAsync(DateTime? effectiveDate = null);

    /// <summary>
    /// Retrieves a legal document by its title.
    /// </summary>
    /// <param name="title">The title of the legal document to retrieve.</param>
    /// <returns>
    /// A <see cref="LegalDocumentDto"/> representing the legal document with the specified title,
    /// or null if no matching document is found.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the provided title is null or empty.</exception>
    Task<LegalDocumentDto?> GetLegalDocumentByTitleAsync(string title);

    /// <summary>
    /// Creates a new legal document based on the provided request and stores it in the repository.
    /// </summary>
    /// <param name="request">An instance of <see cref="LegalDocumentCreateRequest"/> containing the details of the legal document to be created.</param>
    /// <returns>
    /// An instance of <see cref="LegalDocumentDto"/> representing the created legal document;
    /// or <c>null</c> if the operation fails.
    /// </returns>
    Task<LegalDocumentDto?> CreateLegalDocumentAsync(LegalDocumentCreateRequest request);

    /// <summary>
    /// Updates an existing legal document with the provided details.
    /// </summary>
    /// <param name="request">The request containing the updated details of the legal document, including its ID, title, content, effective date, and optionally file content.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated legal document details if the operation succeeds, or null if the document could not be found.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when a legal document with the specified ID does not exist.</exception>
    Task<LegalDocumentDto?> UpdateLegalDocumentAsync(LegalDocumentUpdateRequest request);

    /// <summary>
    /// Deletes a legal document by its unique identifier.
    /// </summary>
    /// <param name="documentId">The unique identifier of the legal document to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteLegalDocumentAsync(Guid documentId);
}