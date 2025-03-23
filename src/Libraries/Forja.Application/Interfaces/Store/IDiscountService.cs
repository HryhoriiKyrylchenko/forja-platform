namespace Forja.Application.Interfaces.Store;

public interface IDiscountService
{
    // Discount Operations
    Task<Discount?> GetDiscountByIdAsync(Guid discountId);
    Task<IEnumerable<Discount>> GetAllDiscountsAsync();
    Task<IEnumerable<Discount>> GetActiveDiscountsAsync(DateTime currentDate);
    Task AddDiscountAsync(DiscountCreateRequest request);
    Task UpdateDiscountAsync(DiscountUpdateRequest request);
    Task DeleteDiscountAsync(Guid discountId);

    // ProductDiscount Operations
    Task<ProductDiscount?> GetProductDiscountByIdAsync(Guid productDiscountId);
    Task<IEnumerable<ProductDiscount>> GetProductDiscountsByProductIdAsync(Guid productId);
    Task<IEnumerable<ProductDiscount>> GetProductDiscountsByDiscountIdAsync(Guid discountId);
    Task AddProductDiscountAsync(ProductDiscountCreateRequest request);
    Task UpdateProductDiscountAsync(ProductDiscountUpdateRequest request);
    Task DeleteProductDiscountAsync(Guid productDiscountId);
}