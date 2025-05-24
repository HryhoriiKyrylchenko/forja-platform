namespace Forja.Application.Interfaces.Store;

/// <summary>
/// Interface defining operations for managing discounts and product-specific discounts.
/// </summary>
public interface IDiscountService
{
    // Discount Operations
    /// <summary>
    /// Retrieves a discount by its unique identifier.
    /// </summary>
    /// <param name="discountId">The unique identifier of the discount to retrieve.</param>
    /// <returns>
    /// A <see cref="DiscountDto"/> representing the discount information, or <c>null</c> if no discount is found with the specified ID.
    /// </returns>
    Task<DiscountDto?> GetDiscountByIdAsync(Guid discountId);

    /// <summary>
    /// Retrieves all available discount records.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="DiscountDto"/> objects, representing all discounts currently stored.
    /// </returns>
    Task<IEnumerable<DiscountDto>> GetAllDiscountsAsync();

    /// <summary>
    /// Retrieves all active discounts based on the current date.
    /// </summary>
    /// <param name="currentDate">The current date used to determine active discounts.</param>
    /// <returns>
    /// A collection of <see cref="DiscountDto"/> objects representing the discounts
    /// that are currently valid and active.
    /// </returns>
    Task<IEnumerable<DiscountDto>> GetActiveDiscountsAsync(DateTime currentDate);

    /// <summary>
    /// Adds a new discount based on the provided request details.
    /// </summary>
    /// <param name="request">The request containing the details required to create a new discount.</param>
    /// <returns>
    /// A <see cref="DiscountDto"/> representing the newly created discount, or <c>null</c> if the creation operation fails.
    /// </returns>
    Task<DiscountDto?> AddDiscountAsync(DiscountCreateRequest request);

    /// <summary>
    /// Updates an existing discount with the provided information.
    /// </summary>
    /// <param name="request">
    /// A <see cref="DiscountUpdateRequest"/> containing the details to update the discount, including its ID and updated properties.
    /// </param>
    /// <returns>
    /// A <see cref="DiscountDto"/> representing the updated discount details, or <c>null</c> if the discount could not be found or updated.
    /// </returns>
    Task<DiscountDto?> UpdateDiscountAsync(DiscountUpdateRequest request);

    /// <summary>
    /// Deletes a discount by its unique identifier.
    /// </summary>
    /// <param name="discountId">The unique identifier of the discount to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    Task DeleteDiscountAsync(Guid discountId);

    // ProductDiscount Operations
    /// <summary>
    /// Retrieves a product-specific discount by its unique identifier.
    /// </summary>
    /// <param name="productDiscountId">The unique identifier of the product discount to retrieve.</param>
    /// <returns>
    /// A <see cref="ProductDiscountDto"/> representing the product discount information, or <c>null</c> if no product discount is found with the specified ID.
    /// </returns>
    Task<ProductDiscountDto?> GetProductDiscountByIdAsync(Guid productDiscountId);

    /// <summary>
    /// Retrieves all discount records associated with a specific product based on the product identifier.
    /// </summary>
    /// <param name="productId">The unique identifier of the product whose discounts are to be retrieved.</param>
    /// <returns>
    /// A collection of <see cref="ProductDiscountDto"/> objects containing the discount details for the specified product.
    /// </returns>
    Task<IEnumerable<ProductDiscountDto>> GetProductDiscountsByProductIdAsync(Guid productId);

    /// <summary>
    /// Retrieves all product discounts associated with a specific discount by its unique identifier.
    /// </summary>
    /// <param name="discountId">The unique identifier of the discount whose associated product discounts are to be retrieved.</param>
    /// <returns>
    /// A collection of <see cref="ProductDiscountDto"/> objects representing the product discounts associated with the specified discount.
    /// </returns>
    Task<IEnumerable<ProductDiscountDto>> GetProductDiscountsByDiscountIdAsync(Guid discountId);

    /// <summary>
    /// Adds a new product-specific discount to the system.
    /// </summary>
    /// <param name="request">The details of the product discount to be added, including the product ID and discount ID.</param>
    /// <returns>
    /// A <see cref="ProductDiscountDto"/> representing the newly created product discount, or <c>null</c> if the operation fails.
    /// </returns>
    Task<ProductDiscountDto?> AddProductDiscountAsync(ProductDiscountCreateRequest request);

    /// <summary>
    /// Updates an existing product discount with new details.
    /// </summary>
    /// <param name="request">The request containing the updated product discount data, including the associated product and discount identifiers.</param>
    /// <returns>
    /// A <see cref="ProductDiscountDto"/> representing the updated product discount, or <c>null</c> if the update operation fails or the product discount does not exist.
    /// </returns>
    Task<ProductDiscountDto?> UpdateProductDiscountAsync(ProductDiscountUpdateRequest request);

    /// <summary>
    /// Deletes a product-specific discount by its unique identifier.
    /// </summary>
    /// <param name="productDiscountId">The unique identifier of the product discount to delete.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation to delete the specified product discount.
    /// </returns>
    Task DeleteProductDiscountAsync(Guid productDiscountId);
}