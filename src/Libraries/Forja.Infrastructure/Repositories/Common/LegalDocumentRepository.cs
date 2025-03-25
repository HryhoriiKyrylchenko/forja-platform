namespace Forja.Infrastructure.Repositories.Common;

/// <summary>
/// Implementation of the LegalDocument repository interface for managing LegalDocument data.
/// </summary>
public class LegalDocumentRepository : ILegalDocumentRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<LegalDocument> _legalDocuments;

    public LegalDocumentRepository(ForjaDbContext context)
    {
        _context = context;
        _legalDocuments = context.Set<LegalDocument>();
    }

    /// <inheritdoc />
    public async Task<LegalDocument?> GetLegalDocumentByIdAsync(Guid documentId)
    {
        if (documentId == Guid.Empty)
        {
            throw new ArgumentException("Invalid document ID.", nameof(documentId));
        }

        return await _legalDocuments.FirstOrDefaultAsync(ld => ld.Id == documentId);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<LegalDocument>> GetLegalDocumentsByEffectiveDateAsync(DateTime? effectiveDate = null)
    {
        var query = _legalDocuments.AsQueryable();

        if (effectiveDate.HasValue)
        {
            query = query.Where(ld => ld.EffectiveDate.Date == effectiveDate.Value.Date);
        }

        return await query.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<LegalDocument?> AddLegalDocumentAsync(LegalDocument document)
    {
        if (!CommonModelValidator.ValidateLegalDocumentModel(document, out var errors))
        {
            throw new ArgumentException($"Invalid legal document. Error: {errors}", nameof(document));
        }

        await _legalDocuments.AddAsync(document);
        await _context.SaveChangesAsync();
        
        return document;
    }

    /// <inheritdoc />
    public async Task<LegalDocument?> UpdateLegalDocumentAsync(LegalDocument document)
    {
        if (!CommonModelValidator.ValidateLegalDocumentModel(document, out var errors))
        {
            throw new ArgumentException($"Invalid legal document. Error: {errors}", nameof(document));
        }

        _legalDocuments.Update(document);
        await _context.SaveChangesAsync();
        
        return document;
    }

    /// <inheritdoc />
    public async Task DeleteLegalDocumentAsync(Guid documentId)
    {
        if (documentId == Guid.Empty)
        {
            throw new ArgumentException("Invalid document ID.", nameof(documentId));
        }

        var document = await _legalDocuments.FirstOrDefaultAsync(ld => ld.Id == documentId);

        if (document == null)
        {
            throw new KeyNotFoundException($"Legal document with ID {documentId} not found.");
        }

        _legalDocuments.Remove(document);
        await _context.SaveChangesAsync();
    }
}
