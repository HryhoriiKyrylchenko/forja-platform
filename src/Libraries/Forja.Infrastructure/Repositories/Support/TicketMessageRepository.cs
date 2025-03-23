namespace Forja.Infrastructure.Repositories.Support;

/// <summary>
/// Repository implementation for managing TicketMessage entities.
/// </summary>
public class TicketMessageRepository : ITicketMessageRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<TicketMessage> _ticketMessages;

    /// <summary>
    /// Initializes a new instance of the <see cref="TicketMessageRepository"/> class.
    /// </summary>
    /// <param name="context">The DbContext instance.</param>
    public TicketMessageRepository(ForjaDbContext context)
    {
        _context = context;
        _ticketMessages = context.Set<TicketMessage>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TicketMessage>> GetAllAsync()
    {
        return await _ticketMessages
            .OrderBy(tm => tm.SentAt)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<TicketMessage?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid TicketMessage ID.", nameof(id));
        }

        return await _ticketMessages
            .FirstOrDefaultAsync(tm => tm.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TicketMessage>> GetBySupportTicketIdAsync(Guid supportTicketId)
    {
        if (supportTicketId == Guid.Empty)
        {
            throw new ArgumentException("Invalid SupportTicket ID.", nameof(supportTicketId));
        }

        return await _ticketMessages
            .Where(tm => tm.SupportTicketId == supportTicketId)
            .OrderBy(tm => tm.SentAt)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<TicketMessage?> AddAsync(TicketMessage ticketMessage)
    {
        if (!SupportModelValidator.ValidateTicketMessageModel(ticketMessage, out string? errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }

        await _ticketMessages.AddAsync(ticketMessage);
        await _context.SaveChangesAsync();
        return ticketMessage;
    }

    /// <inheritdoc />
    public async Task<TicketMessage?> UpdateAsync(TicketMessage ticketMessage)
    {
        if (!SupportModelValidator.ValidateTicketMessageModel(ticketMessage, out string? errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }

        _ticketMessages.Update(ticketMessage);
        await _context.SaveChangesAsync();
        return ticketMessage;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid TicketMessage ID.", nameof(id));
        }

        var message = await GetByIdAsync(id);
        if (message != null)
        {
            _ticketMessages.Remove(message);
            await _context.SaveChangesAsync();
        }
    }
}