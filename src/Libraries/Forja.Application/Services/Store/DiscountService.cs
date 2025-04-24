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

    ///<inheritdoc/>
    public async Task<DiscountDto?> GetDiscountByIdAsync(Guid discountId)
    {
        if (discountId == Guid.Empty)
        {
            throw new ArgumentException("Discount ID cannot be empty.", nameof(discountId));
        }

        var discount = await _discountRepository.GetDiscountByIdAsync(discountId);
        
        return discount == null ? null : StoreEntityToDtoMapper.MapToDiscountDto(discount);
    }

    ///<inheritdoc/>
    public async Task<IEnumerable<DiscountDto>> GetAllDiscountsAsync()
    {
        var discounts = await _discountRepository.GetAllDiscountsAsync();
        
        return discounts.Select(StoreEntityToDtoMapper.MapToDiscountDto);
    }

    ///<inheritdoc/>
    public async Task<IEnumerable<DiscountDto>> GetActiveDiscountsAsync(DateTime currentDate)
    {
        var activeDiscounts =  await _discountRepository.GetActiveDiscountsAsync(currentDate);
        
        return activeDiscounts.Select(StoreEntityToDtoMapper.MapToDiscountDto);
    }

    ///<inheritdoc/>
    public async Task<DiscountDto?> AddDiscountAsync(DiscountCreateRequest request)
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

        var result = await _discountRepository.AddDiscountAsync(discount);
        
        return result == null ? null : StoreEntityToDtoMapper.MapToDiscountDto(result);
    }

    ///<inheritdoc/>
    public async Task<DiscountDto?> UpdateDiscountAsync(DiscountUpdateRequest request)
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

        var result = await _discountRepository.UpdateDiscountAsync(discount);
        
        return result == null ? null : StoreEntityToDtoMapper.MapToDiscountDto(result);
    }

    ///<inheritdoc/>
    public async Task DeleteDiscountAsync(Guid discountId)
    {
        if (discountId == Guid.Empty)
        {
            throw new ArgumentException("Discount ID cannot be empty.", nameof(discountId));
        }

        await _discountRepository.DeleteDiscountAsync(discountId);
    }

    // ---------------- ProductDiscount Operations ----------------

    ///<inheritdoc/>
    public async Task<ProductDiscountDto?> GetProductDiscountByIdAsync(Guid productDiscountId)
    {
        if (productDiscountId == Guid.Empty)
        {
            throw new ArgumentException("ProductDiscount ID cannot be empty.", nameof(productDiscountId));
        }

        var productDiscount = await _productDiscountRepository.GetProductDiscountByIdAsync(productDiscountId);
        
        return productDiscount == null ? null : StoreEntityToDtoMapper.MapToProductDiscountDto(productDiscount);
    }

    ///<inheritdoc/>
    public async Task<IEnumerable<ProductDiscountDto>> GetProductDiscountsByProductIdAsync(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
        }

        var productDiscounts = await _productDiscountRepository.GetProductDiscountsByProductIdAsync(productId);
        
        return productDiscounts.Select(StoreEntityToDtoMapper.MapToProductDiscountDto);
    }

    ///<inheritdoc/>
    public async Task<IEnumerable<ProductDiscountDto>> GetProductDiscountsByDiscountIdAsync(Guid discountId)
    {
        if (discountId == Guid.Empty)
        {
            throw new ArgumentException("Discount ID cannot be empty.", nameof(discountId));
        }

        var productDiscounts = await _productDiscountRepository.GetProductDiscountsByDiscountIdAsync(discountId);
        
        return productDiscounts.Select(StoreEntityToDtoMapper.MapToProductDiscountDto);
    }

    ///<inheritdoc/>
    public async Task<ProductDiscountDto?> AddProductDiscountAsync(ProductDiscountCreateRequest request)
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

        var result = await _productDiscountRepository.AddProductDiscountAsync(productDiscount);
        
        return result == null ? null : StoreEntityToDtoMapper.MapToProductDiscountDto(result);
    }

    ///<inheritdoc/>
    public async Task<ProductDiscountDto?> UpdateProductDiscountAsync(ProductDiscountUpdateRequest request)
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

        var result = await _productDiscountRepository.UpdateProductDiscountAsync(productDiscount);
        
        return result == null ? null : StoreEntityToDtoMapper.MapToProductDiscountDto(result);
    }

    ///<inheritdoc/>
    public async Task DeleteProductDiscountAsync(Guid productDiscountId)
    {
        if (productDiscountId == Guid.Empty)
        {
            throw new ArgumentException("ProductDiscount ID cannot be empty.", nameof(productDiscountId));
        }

        await _productDiscountRepository.DeleteProductDiscountAsync(productDiscountId);
    }
}