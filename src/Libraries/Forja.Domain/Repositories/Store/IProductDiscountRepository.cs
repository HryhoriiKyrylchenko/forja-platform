namespace Forja.Domain.Repositories.Store;

/// <summary>
/// Interface for the ProductDiscount repository, defining methods for ProductDiscount CRUD operations.
/// </summary>
public interface IProductDiscountRepository
{
    /// <summary>
    /// Retrieves a ProductDiscount by its unique identifier.
    /// </summary>
    /// <param name="productDiscountId">The unique ID of the ProductDiscount.</param>
    /// <returns>The matching ProductDiscount object or null if not found.</returns>
    Task<ProductDiscount?> GetProductDiscountByIdAsync(Guid productDiscountId);

    /// <summary>
    /// Retrieves all ProductDiscounts associated with a specific product.
    /// </summary>
    /// <param name="productId">The unique ID of the product.</param>
    /// <returns>A list of ProductDiscounts for the specified product.</returns>
    Task<IEnumerable<ProductDiscount>> GetProductDiscountsByProductIdAsync(Guid productId);

    /// <summary>
    /// Retrieves all ProductDiscounts associated with a specific discount.
    /// </summary>
    /// <param name="discountId">The unique ID of the discount.</param>
    /// <returns>A list of ProductDiscounts for the specified discount.</returns>
    Task<IEnumerable<ProductDiscount>> GetProductDiscountsByDiscountIdAsync(Guid discountId);

    /// <summary>
    /// Adds a new ProductDiscount to the database.
    /// </summary>
    /// <param name="productDiscount">The ProductDiscount object to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<ProductDiscount?> AddProductDiscountAsync(ProductDiscount productDiscount);

    /// <summary>
    /// Updates an existing ProductDiscount in the database.
    /// </summary>
    /// <param name="productDiscount">The ProductDiscount object to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<ProductDiscount?> UpdateProductDiscountAsync(ProductDiscount productDiscount);

    /// <summary>
    /// Deletes a ProductDiscount from the database by its unique identifier.
    /// </summary>
    /// <param name="productDiscountId">The unique ID of the ProductDiscount to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteProductDiscountAsync(Guid productDiscountId);
}