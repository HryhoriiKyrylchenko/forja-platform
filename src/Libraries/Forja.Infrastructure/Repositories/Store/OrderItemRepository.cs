namespace Forja.Infrastructure.Repositories.Store;

/// <summary>
/// Implementation of the OrderItem repository interface for managing OrderItem data.
/// </summary>
public class OrderItemRepository : IOrderItemRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<OrderItem> _orderItems;

    /// <summary>
    /// Initializes a new instance of the OrderItemRepository class.
    /// </summary>
    /// <param name="context">The database context to use for operations.</param>
    public OrderItemRepository(ForjaDbContext context)
    {
        _context = context;
        _orderItems = context.Set<OrderItem>();
    }

    /// <summary>
    /// Retrieves an OrderItem by its unique identifier.
    /// </summary>
    /// <param name="orderItemId">The unique identifier of the OrderItem.</param>
    /// <returns>The matching OrderItem object or null if not found.</returns>
    public async Task<OrderItem?> GetOrderItemByIdAsync(Guid orderItemId)
    {
        if (orderItemId == Guid.Empty)
        {
            throw new ArgumentException("Invalid order item ID.", nameof(orderItemId));
        }

        return await _orderItems
            .Where(oi => !oi.IsDeleted)
            .Include(oi => oi.Product)
            .FirstOrDefaultAsync(oi => oi.Id == orderItemId);
    }

    /// <summary>
    /// Retrieves all OrderItems associated with a specific Order.
    /// </summary>
    /// <param name="orderId">The unique identifier of the Order.</param>
    /// <returns>A collection of OrderItems for the specified Order.</returns>
    public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(Guid orderId)
    {
        if (orderId == Guid.Empty)
        {
            throw new ArgumentException("Invalid order ID.", nameof(orderId));
        }

        return await _orderItems
            .Where(oi => !oi.IsDeleted)
            .Include(oi => oi.Product)
            .Where(oi => oi.OrderId == orderId)
            .ToListAsync();
    }

    /// <summary>
    /// Adds a new OrderItem to the database.
    /// </summary>
    /// <param name="orderItem">The OrderItem object to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AddOrderItemAsync(OrderItem orderItem)
    {
        if (!StoreModelValidator.ValidateOrderItemModel(orderItem, out string errors))
        {
            throw new ArgumentException(errors);
        }

        await _orderItems.AddAsync(orderItem);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing OrderItem in the database.
    /// </summary>
    /// <param name="orderItem">The OrderItem object to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UpdateOrderItemAsync(OrderItem orderItem)
    {
        if (!StoreModelValidator.ValidateOrderItemModel(orderItem, out string errors))
        {
            throw new ArgumentException(errors);
        }

        _orderItems.Update(orderItem);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an OrderItem from the database by its unique identifier.
    /// </summary>
    /// <param name="orderItemId">The unique identifier of the OrderItem to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteOrderItemAsync(Guid orderItemId)
    {
        if (orderItemId == Guid.Empty)
        {
            throw new ArgumentException("Invalid order item ID.", nameof(orderItemId));
        }

        var orderItem = await _orderItems.FindAsync(orderItemId);
        if (orderItem == null)
        {
            throw new KeyNotFoundException($"OrderItem with ID {orderItemId} not found.");
        }

        orderItem.IsDeleted = true;
        _orderItems.Update(orderItem);
        await _context.SaveChangesAsync();
    }
}