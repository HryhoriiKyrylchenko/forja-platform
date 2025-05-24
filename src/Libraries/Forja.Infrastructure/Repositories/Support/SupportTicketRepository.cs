namespace Forja.Infrastructure.Repositories.Support;

/// <summary>
/// Repository implementation for managing SupportTicket entities.
/// </summary>
public class SupportTicketRepository : ISupportTicketRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<SupportTicket> _tickets;

    /// <summary>
    /// Initializes a new instance of the <see cref="SupportTicketRepository"/> class.
    /// </summary>
    /// <param name="context">The DbContext instance.</param>
    public SupportTicketRepository(ForjaDbContext context)
    {
        _context = context;
        _tickets = context.Set<SupportTicket>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SupportTicket>> GetAllAsync()
    {
        return await _tickets 
            .Include(t => t.Messages) 
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<SupportTicket?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid SupportTicket ID.", nameof(id));
        }

        return await _tickets
            .Include(t => t.Messages)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SupportTicket>> GetByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Invalid User ID.", nameof(userId));
        }

        return await _tickets
            .Include(t => t.Messages)
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<SupportTicket?> AddAsync(SupportTicket ticket)
    {
        if (!SupportModelValidator.ValidateSupportTicketModel(ticket, out string? errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }

        await _tickets.AddAsync(ticket);
        await _context.SaveChangesAsync();
        return ticket;
    }

    /// <inheritdoc />
    public async Task<SupportTicket?> UpdateAsync(SupportTicket ticket)
    {
        if (!SupportModelValidator.ValidateSupportTicketModel(ticket, out string? errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }

        _tickets.Update(ticket);
        await _context.SaveChangesAsync();
        return ticket;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid SupportTicket ID.", nameof(id));
        }

        var ticket = await GetByIdAsync(id);
        if (ticket != null)
        {
            _tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }
    }
}