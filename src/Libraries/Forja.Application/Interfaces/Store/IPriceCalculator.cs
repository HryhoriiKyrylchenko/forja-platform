namespace Forja.Application.Interfaces.Store;

/// <summary>
/// Interface for performing price-related calculations such as applying discounts,
/// calculating total prices, and comparing price differences.
/// </summary>
public interface IPriceCalculator
{
    /// <summary>
    /// Applies the given product discounts to the original price and calculates the final discounted price.
    /// </summary>
    /// <param name="originalPrice">The original price of the product before any discounts are applied.</param>
    /// <param name="productDiscounts">A collection of applicable product discounts to apply to the original price.</param>
    /// <returns>The final price after applying all the specified discounts.</returns>
    decimal ApplyDiscount(decimal originalPrice, IEnumerable<ProductDiscount> productDiscounts);

    /// <summary>
    /// Calculates the total price for a set of cart items by applying any applicable product discounts.
    /// </summary>
    /// <param name="cartItems">The collection of cart items for which the total price needs to be calculated.</param>
    /// <param name="productDiscountRepository">The repository used to fetch product discounts applicable to the cart items.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the calculated total price.</returns>
    Task<decimal> CalculateTotalAsync(IEnumerable<CartItem> cartItems,
        IProductDiscountRepository productDiscountRepository);

    /// <summary>
    /// Determines if two prices are different within a specified tolerance value.
    /// </summary>
    /// <param name="price1">
    /// The first price to compare.
    /// </param>
    /// <param name="price2">
    /// The second price to compare.
    /// </param>
    /// <param name="tolerance">
    /// The allowable difference between the two prices for them to be considered the same.
    /// Defaults to 0.01.
    /// </param>
    /// <returns>
    /// A boolean value indicating whether the two prices are considered different
    /// based on the specified tolerance.
    /// </returns>
    bool ArePricesDifferent(decimal price1, decimal price2, decimal tolerance = 0.01m);
}