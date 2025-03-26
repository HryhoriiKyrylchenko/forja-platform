namespace Forja.Infrastructure.Repositories.Analytics;

/// <summary>
/// Implementation of the IAnalyticsAggregateRepository interface.
/// Provides data operations for AnalyticsAggregates.
/// </summary>
public class AnalyticsAggregateRepository : IAnalyticsAggregateRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<AnalyticsAggregate> _analyticsAggregates;

    public AnalyticsAggregateRepository(ForjaDbContext context)
    {
        _context = context;
        _analyticsAggregates = context.Set<AnalyticsAggregate>();
    }

    /// <inheritdoc />
    public async Task<AnalyticsAggregate?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid analytics aggregate ID.", nameof(id));
        }

        return await _analyticsAggregates.FirstOrDefaultAsync(aa => aa.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AnalyticsAggregate>> GetAllAsync()
    {
        return await _analyticsAggregates.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<AnalyticsAggregate?> AddAsync(AnalyticsAggregate aggregate)
    {
        if (!AnalyticsModelValidator.ValidateAnalyticsAggregateModel(aggregate, out string? error))
        {
            throw new ArgumentException(error);
        }

        await _analyticsAggregates.AddAsync(aggregate);
        await _context.SaveChangesAsync();
        
        return aggregate;
    }

    /// <inheritdoc />
    public async Task<AnalyticsAggregate?> UpdateAsync(AnalyticsAggregate aggregate)
    {
        if (!AnalyticsModelValidator.ValidateAnalyticsAggregateModel(aggregate, out string? error))
        {
            throw new ArgumentException(error);
        }

        _analyticsAggregates.Update(aggregate);
        await _context.SaveChangesAsync();
        
        return aggregate;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid analytics aggregate ID.", nameof(id));
        }

        var aggregate = await _analyticsAggregates.FirstOrDefaultAsync(aa => aa.Id == id);

        if (aggregate == null)
        {
            throw new KeyNotFoundException($"Analytics aggregate with ID {id} not found.");
        }

        _analyticsAggregates.Remove(aggregate);
        await _context.SaveChangesAsync();
    }
}