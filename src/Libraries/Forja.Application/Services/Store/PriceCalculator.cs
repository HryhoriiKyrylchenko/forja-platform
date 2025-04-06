namespace Forja.Application.Services.Store;

public class PriceCalculator : IPriceCalculator
{
    public decimal ApplyDiscount(decimal originalPrice, IEnumerable<ProductDiscount> productDiscounts)
    {
        decimal discountedPrice = originalPrice;

        foreach (var productDiscount in productDiscounts)
        {
            var discount = productDiscount.Discount;

            if (discount.StartDate.HasValue && DateTime.UtcNow < discount.StartDate.Value)
                continue; 

            if (discount.EndDate.HasValue && DateTime.UtcNow > discount.EndDate.Value)
                continue; 

            if (discount.DiscountType == DiscountType.Percentage)
            {
                discountedPrice -= originalPrice * (discount.DiscountValue / 100);
            }
            else if (discount.DiscountType == DiscountType.Fixed)
            {
                discountedPrice -= discount.DiscountValue;
            }

            if (discountedPrice < 0)
                discountedPrice = 0.01m; 
        }

        return discountedPrice;
    }

    public async Task<decimal> CalculateTotalAsync(IEnumerable<CartItem> cartItems, IProductDiscountRepository productDiscountRepository)
    {
        decimal totalPrice = 0;

        foreach (var cartItem in cartItems)
        {
            var productDiscounts = await productDiscountRepository.GetProductDiscountsByProductIdAsync(cartItem.ProductId);
            var priceAfterDiscount = ApplyDiscount(cartItem.Price, productDiscounts);
            totalPrice += priceAfterDiscount;
        }

        return totalPrice;
    }
    
    public bool ArePricesDifferent(decimal price1, decimal price2, decimal tolerance = 0.01m)
    {
        return Math.Abs(price1 - price2) > tolerance;
    }
}