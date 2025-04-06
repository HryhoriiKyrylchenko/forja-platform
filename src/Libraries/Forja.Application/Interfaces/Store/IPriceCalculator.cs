namespace Forja.Application.Interfaces.Store;

public interface IPriceCalculator
{
    decimal ApplyDiscount(decimal originalPrice, IEnumerable<ProductDiscount> productDiscounts);

    Task<decimal> CalculateTotalAsync(IEnumerable<CartItem> cartItems,
        IProductDiscountRepository productDiscountRepository);

    bool ArePricesDifferent(decimal price1, decimal price2, decimal tolerance = 0.01m);
}