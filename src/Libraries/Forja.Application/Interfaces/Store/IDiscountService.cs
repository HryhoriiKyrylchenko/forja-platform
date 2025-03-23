namespace Forja.Application.Interfaces.Store;

public interface IDiscountService
{
    // Discount Operations
    Task<DiscountDto?> GetDiscountByIdAsync(Guid discountId);
    Task<IEnumerable<DiscountDto>> GetAllDiscountsAsync();
    Task<IEnumerable<DiscountDto>> GetActiveDiscountsAsync(DateTime currentDate);
    Task<DiscountDto?> AddDiscountAsync(DiscountCreateRequest request);
    Task<DiscountDto?> UpdateDiscountAsync(DiscountUpdateRequest request);
    Task DeleteDiscountAsync(Guid discountId);

    // ProductDiscount Operations
    Task<ProductDiscountDto?> GetProductDiscountByIdAsync(Guid productDiscountId);
    Task<IEnumerable<ProductDiscountDto>> GetProductDiscountsByProductIdAsync(Guid productId);
    Task<IEnumerable<ProductDiscountDto>> GetProductDiscountsByDiscountIdAsync(Guid discountId);
    Task<ProductDiscountDto?> AddProductDiscountAsync(ProductDiscountCreateRequest request);
    Task<ProductDiscountDto?> UpdateProductDiscountAsync(ProductDiscountUpdateRequest request);
    Task DeleteProductDiscountAsync(Guid productDiscountId);
}