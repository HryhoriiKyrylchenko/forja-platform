namespace Forja.Domain.Repositories.Store;

/// <summary>
/// Interface for the Discount repository, defining methods for Discount CRUD operations.
/// </summary>
public interface IDiscountRepository
{
    /// <summary>
    /// Retrieves a Discount by its unique identifier.
    /// </summary>
    /// <param name="discountId">The unique identifier of the Discount.</param>
    /// <returns>The matching Discount object or null if not found.</returns>
    Task<Discount?> GetDiscountByIdAsync(Guid discountId);

    /// <summary>
    /// Retrieves all Discounts in the database.
    /// </summary>
    /// <returns>A list of all Discounts.</returns>
    Task<IEnumerable<Discount>> GetAllDiscountsAsync();

    /// <summary>
    /// Retrieves active Discounts in the current time period.
    /// </summary>
    /// <param name="currentDate">The current date as a reference point.</param>
    /// <returns>A list of Discounts that are currently active.</returns>
    Task<IEnumerable<Discount>> GetActiveDiscountsAsync(DateTime currentDate);

    /// <summary>
    /// Adds a new Discount to the database.
    /// </summary>
    /// <param name="discount">The Discount to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddDiscountAsync(Discount discount);

    /// <summary>
    /// Updates an existing Discount in the database.
    /// </summary>
    /// <param name="discount">The Discount to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateDiscountAsync(Discount discount);

    /// <summary>
    /// Deletes a Discount from the database by its ID.
    /// </summary>
    /// <param name="discountId">The ID of the Discount to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteDiscountAsync(Guid discountId);
}