namespace Forja.Application.Services.Store;

public class DiscountService : IDiscountService
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IProductDiscountRepository _productDiscountRepository;

    public DiscountService(IDiscountRepository discountRepository, IProductDiscountRepository productDiscountRepository)
    {
        _discountRepository = discountRepository;
        _productDiscountRepository = productDiscountRepository;
    }

    // ---------------- Discount Operations ----------------

    public async Task<Discount?> GetDiscountByIdAsync(Guid discountId)
    {
        if (discountId == Guid.Empty)
        {
            throw new ArgumentException("Discount ID cannot be empty.", nameof(discountId));
        }

        return await _discountRepository.GetDiscountByIdAsync(discountId);
    }

    public async Task<IEnumerable<Discount>> GetAllDiscountsAsync()
    {
        return await _discountRepository.GetAllDiscountsAsync();
    }

    public async Task<IEnumerable<Discount>> GetActiveDiscountsAsync(DateTime currentDate)
    {
        return await _discountRepository.GetActiveDiscountsAsync(currentDate);
    }

    public async Task AddDiscountAsync(DiscountCreateRequest request)
    {
        if (!StoreRequestsValidator.ValidateDiscountCreateRequest(request, out string? errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }

        var discount = new Discount
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            DiscountType = request.DiscountType,
            DiscountValue = request.DiscountValue,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            ProductDiscounts = new List<ProductDiscount>()
        };

        await _discountRepository.AddDiscountAsync(discount);
    }

    public async Task UpdateDiscountAsync(DiscountUpdateRequest request)
    {
        if (!StoreRequestsValidator.ValidateDiscountUpdateRequest(request, out string? errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }

        var discount = await _discountRepository.GetDiscountByIdAsync(request.Id);
        if (discount == null)
        {
            throw new KeyNotFoundException($"Discount with ID {request.Id} not found.");
        }

        discount.Name = request.Name;
        discount.DiscountType = request.DiscountType;
        discount.DiscountValue = request.DiscountValue;
        discount.StartDate = request.StartDate;
        discount.EndDate = request.EndDate;

        await _discountRepository.UpdateDiscountAsync(discount);
    }

    public async Task DeleteDiscountAsync(Guid discountId)
    {
        if (discountId == Guid.Empty)
        {
            throw new ArgumentException("Discount ID cannot be empty.", nameof(discountId));
        }

        await _discountRepository.DeleteDiscountAsync(discountId);
    }

    // ---------------- ProductDiscount Operations ----------------

    public async Task<ProductDiscount?> GetProductDiscountByIdAsync(Guid productDiscountId)
    {
        if (productDiscountId == Guid.Empty)
        {
            throw new ArgumentException("ProductDiscount ID cannot be empty.", nameof(productDiscountId));
        }

        return await _productDiscountRepository.GetProductDiscountByIdAsync(productDiscountId);
    }

    public async Task<IEnumerable<ProductDiscount>> GetProductDiscountsByProductIdAsync(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
        }

        return await _productDiscountRepository.GetProductDiscountsByProductIdAsync(productId);
    }

    public async Task<IEnumerable<ProductDiscount>> GetProductDiscountsByDiscountIdAsync(Guid discountId)
    {
        if (discountId == Guid.Empty)
        {
            throw new ArgumentException("Discount ID cannot be empty.", nameof(discountId));
        }

        return await _productDiscountRepository.GetProductDiscountsByDiscountIdAsync(discountId);
    }

    public async Task AddProductDiscountAsync(ProductDiscountCreateRequest request)
    {
        if (!StoreRequestsValidator.ValidateProductDiscountCreateRequest(request, out string? errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }

        var productDiscount = new ProductDiscount
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            DiscountId = request.DiscountId
        };

        await _productDiscountRepository.AddProductDiscountAsync(productDiscount);
    }

    public async Task UpdateProductDiscountAsync(ProductDiscountUpdateRequest request)
    {
        if (!StoreRequestsValidator.ValidateProductDiscountUpdateRequest(request, out string? errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }

        var productDiscount = await _productDiscountRepository.GetProductDiscountByIdAsync(request.Id);
        if (productDiscount == null)
        {
            throw new KeyNotFoundException($"ProductDiscount with ID {request.Id} not found.");
        }

        productDiscount.ProductId = request.ProductId;
        productDiscount.DiscountId = request.DiscountId;

        await _productDiscountRepository.UpdateProductDiscountAsync(productDiscount);
    }

    public async Task DeleteProductDiscountAsync(Guid productDiscountId)
    {
        if (productDiscountId == Guid.Empty)
        {
            throw new ArgumentException("ProductDiscount ID cannot be empty.", nameof(productDiscountId));
        }

        await _productDiscountRepository.DeleteProductDiscountAsync(productDiscountId);
    }
}