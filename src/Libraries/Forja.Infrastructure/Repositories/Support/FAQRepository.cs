namespace Forja.Infrastructure.Repositories.Support;

/// <summary>
/// Repository implementation for managing FAQ entities.
/// </summary>
public class FAQRepository : IFAQRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<FAQ> _faqs;

    /// <summary>
    /// Initializes a new instance of the <see cref="FAQRepository"/> class.
    /// </summary>
    /// <param name="context">The DbContext instance.</param>
    public FAQRepository(ForjaDbContext context)
    {
        _context = context;
        _faqs = context.Set<FAQ>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<FAQ>> GetAllAsync()
    {
        return await _faqs
            .OrderBy(f => f.Order)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<FAQ?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid FAQ ID.", nameof(id));
        }
        return await _faqs.FindAsync(id);
    }

    /// <inheritdoc />
    public async Task<FAQ?> AddAsync(FAQ faq)
    {
        if (!SupportModelValidator.ValidateFAQModel(faq, out string? errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }
        
        await _faqs.AddAsync(faq);
        await _context.SaveChangesAsync();
        return faq;
    }

    /// <inheritdoc />
    public async Task<FAQ?> UpdateAsync(FAQ faq)
    {
        if (!SupportModelValidator.ValidateFAQModel(faq, out string? errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }
        
        _faqs.Update(faq);
        await _context.SaveChangesAsync();
        return faq;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid FAQ ID.", nameof(id));
        }
        var faq = await GetByIdAsync(id);
        if (faq != null)
        {
            _faqs.Remove(faq);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task ReorderAsync(IEnumerable<FAQ> orderedFaqs)
    {
        var faqsToUpdate = orderedFaqs.ToList();

        foreach (var faq in faqsToUpdate)
        {
            var existingFaq = await GetByIdAsync(faq.Id);
            if (existingFaq != null)
            {
                existingFaq.Order = faq.Order;
                _faqs.Update(existingFaq);
            }
        }

        await _context.SaveChangesAsync();
    }
}
