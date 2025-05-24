namespace Forja.Infrastructure.Repositories.Store;

/// <summary>
/// Implementation of the Order repository interface for managing Order data.
/// </summary>
public class OrderRepository : IOrderRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<Order> _orders;

    /// <summary>
    /// Initializes a new instance of the OrderRepository class.
    /// </summary>
    /// <param name="context">The database context to use for operations.</param>
    public OrderRepository(ForjaDbContext context)
    {
        _context = context;
        _orders = context.Set<Order>();
    }
    
    /// <summary>
    /// Retrieves an Order by its unique identifier.
    /// </summary>
    /// <param name="orderId">The unique identifier of the Order.</param>
    /// <returns>The matching Order object or null if not found.</returns>
    public async Task<Order?> GetOrderByIdAsync(Guid orderId)
    {
        if (orderId == Guid.Empty)
        {
            throw new ArgumentException("Invalid order ID.", nameof(orderId));
        }

        return await _orders
            .Where(o => !o.IsDeleted)
            .Include(o => o.Cart)
                .ThenInclude(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
            .Include(o => o.Cart)
            .Include(o => o.Payment)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    /// <summary>
    /// Retrieves all Orders placed by a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of all Orders placed by the specified user.</returns>
    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Invalid user ID.", nameof(userId));
        }

        return await _orders
            .Where(o => !o.IsDeleted)
            .Include(o => o.Cart)
                .ThenInclude(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
            .Where(o => o.Cart.UserId == userId)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves all Orders in the database.
    /// </summary>
    /// <returns>A list of all Orders.</returns>
    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orders
            .Where(o => !o.IsDeleted)
            .Include(o => o.Cart)
                .ThenInclude(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
            .ToListAsync();
    }

    /// <summary>
    /// Adds a new Order to the database.
    /// </summary>
    /// <param name="order">The Order object to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<Order?> AddOrderAsync(Order order)
    {
        if (!StoreModelValidator.ValidateOrderModel(order, out string errors))
        {
            throw new ArgumentException(errors);
        }

        await _orders.AddAsync(order);
        await _context.SaveChangesAsync();
        
        return order;
    }

    /// <summary>
    /// Updates an existing Order in the database.
    /// </summary>
    /// <param name="order">The Order object to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<Order?> UpdateOrderAsync(Order order)
    {
        if (!StoreModelValidator.ValidateOrderModel(order, out string errors))
        {
            throw new ArgumentException(errors);
        }

        _orders.Update(order);
        await _context.SaveChangesAsync();
        
        return order;
    }

    /// <summary>
    /// Deletes an Order from the database by its unique identifier.
    /// </summary>
    /// <param name="orderId">The unique identifier of the Order to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteOrderAsync(Guid orderId)
    {
        if (orderId == Guid.Empty)
        {
            throw new ArgumentException("Invalid order ID.", nameof(orderId));
        }

        var order = await _orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {orderId} not found.");
        }
        
        order.IsDeleted = true;
        _orders.Update(order);
        await _context.SaveChangesAsync();
    }
}