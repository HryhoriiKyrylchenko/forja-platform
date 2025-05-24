namespace Forja.Application.Services.Common;

/// <summary>
/// Service implementation for handling operations related to LegalDocuments.
/// </summary>
public class LegalDocumentService : ILegalDocumentService
{
    private readonly ILegalDocumentRepository _legalDocumentRepository;

    public LegalDocumentService(ILegalDocumentRepository legalDocumentRepository)
    {
        _legalDocumentRepository = legalDocumentRepository;
    }

    /// <inheritdoc />
    public async Task<LegalDocumentDto?> GetLegalDocumentByIdAsync(Guid documentId)
    {
        if (documentId == Guid.Empty)
        {
            throw new ArgumentException("Document ID cannot be empty.", nameof(documentId));
        }

        var document = await _legalDocumentRepository.GetLegalDocumentByIdAsync(documentId);

        return document == null ? null : CommonEntityToDtoMapper.MapToLegalDocumentDto(document);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<LegalDocumentDto>> GetLegalDocumentsByEffectiveDateAsync(DateTime? effectiveDate = null)
    {
        var documents = await _legalDocumentRepository.GetLegalDocumentsByEffectiveDateAsync(effectiveDate);

        return documents.Select(CommonEntityToDtoMapper.MapToLegalDocumentDto);
    }

    /// <inheritdoc />
    public async Task<LegalDocumentDto?> GetLegalDocumentByTitleAsync(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be empty or null.", nameof(title));
        }

        var document = await _legalDocumentRepository.GetLegalDocumentsByTitleAsync(title);

        return document == null ? null : CommonEntityToDtoMapper.MapToLegalDocumentDto(document);
    }

    /// <inheritdoc />
    public async Task<LegalDocumentDto?> CreateLegalDocumentAsync(LegalDocumentCreateRequest request)
    {
        if (!CommonRequestsValidator.ValidateLegalDocumentCreateRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        var document = new LegalDocument
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Content = request.Content,
            EffectiveDate = request.EffectiveDate,
            FileContent = request.FileContent
        };

        var createdDocument = await _legalDocumentRepository.AddLegalDocumentAsync(document);

        return createdDocument == null ? null : CommonEntityToDtoMapper.MapToLegalDocumentDto(createdDocument);
    }

    /// <inheritdoc />
    public async Task<LegalDocumentDto?> UpdateLegalDocumentAsync(LegalDocumentUpdateRequest request)
    {
        if (!CommonRequestsValidator.ValidateLegalDocumentUpdateRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        var document = await _legalDocumentRepository.GetLegalDocumentByIdAsync(request.Id);

        if (document == null)
        {
            throw new KeyNotFoundException($"Legal document with ID {request.Id} not found.");
        }

        document.Title = request.Title;
        document.Content = request.Content;
        document.EffectiveDate = request.EffectiveDate;
        document.FileContent = request.FileContent;

        var updatedDocument = await _legalDocumentRepository.UpdateLegalDocumentAsync(document);

        return updatedDocument == null ? null : CommonEntityToDtoMapper.MapToLegalDocumentDto(updatedDocument);
    }

    /// <inheritdoc />
    public async Task DeleteLegalDocumentAsync(Guid documentId)
    {
        if (documentId == Guid.Empty)
        {
            throw new ArgumentException("Document ID cannot be empty.", nameof(documentId));
        }

        await _legalDocumentRepository.DeleteLegalDocumentAsync(documentId);
    }
}