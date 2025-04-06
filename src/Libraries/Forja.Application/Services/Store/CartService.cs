namespace Forja.Application.Services.Store;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IProductDiscountRepository _productDiscountRepository;
    private readonly IPriceCalculator _priceCalculator;
    private readonly IProductRepository _productRepository;
    private readonly IBundleRepository _bundleRepository;
    private readonly IBundleProductRepository _bundleProductRepository;
    private readonly IFileManagerService _fileManagerService;

    public CartService(ICartRepository cartRepository, 
        ICartItemRepository cartItemRepository,
        IProductDiscountRepository productDiscountRepository,
        IPriceCalculator priceCalculator,
        IProductRepository productRepository,
        IBundleRepository bundleRepository,
        IBundleProductRepository bundleProductRepository,
        IFileManagerService fileManagerService)
    {
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
        _productDiscountRepository = productDiscountRepository;
        _priceCalculator = priceCalculator;
        _productRepository = productRepository;
        _bundleRepository = bundleRepository;
        _bundleProductRepository = bundleProductRepository;
        _fileManagerService = fileManagerService;
    }

    // ---------------- Cart Operations --------------------

    public async Task<CartDto?> GetCartByIdAsync(Guid cartId)
    {
        if (cartId == Guid.Empty)
        {
            throw new ArgumentException("Cart ID cannot be empty.", nameof(cartId));
        }

        var cart = await _cartRepository.GetCartByIdAsync(cartId);
        if (cart == null) return null;
        
        var cartItems = await GetCartItemsByCartIdAsync(cart.Id);
        
        return StoreEntityToDtoMapper.MapToCartDto(cart, cartItems);
    }

    public async Task<IEnumerable<CartDto>> GetCartsByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        }
        
        var carts = await _cartRepository.GetCartsByUserIdAsync(userId);
        var cartsList = carts.ToList();
        
        var cartDtos = new List<CartDto>();

        foreach (var cart in cartsList)
        {
            var cartItems = await GetCartItemsByCartIdAsync(cart.Id);

            cartDtos.Add(StoreEntityToDtoMapper.MapToCartDto(cart, cartItems));
        }

        return cartDtos;
    }

    public async Task<CartDto> GetOrCreateActiveCartAsync(CartCreateRequest request)
    {
        if (!StoreRequestsValidator.ValidateCartCreateRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Errors: {errors}", nameof(request));
        }
        
        var activeCart = await _cartRepository.GetActiveCartByUserIdAsync(request.UserId);

        if (activeCart == null)
        {
            activeCart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Status = CartStatus.Active,
                CreatedAt = DateTime.UtcNow
            };
            
            await _cartRepository.AddCartAsync(activeCart);
            
            return StoreEntityToDtoMapper.MapToCartDto(activeCart, new List<CartItemDto>());
        }

        if (!await IsCartRelevantAsync(activeCart.Id))
        {
            return await UpdateCartAsync(activeCart.Id) ?? throw new InvalidOperationException("Cannot update cart.");
        }
            
        var cartItems = await GetCartItemsByCartIdAsync(activeCart.Id);
        return StoreEntityToDtoMapper.MapToCartDto(activeCart, cartItems);
        
    }

    public async Task RemoveCartAsync(Guid cartId)
    {
        if (cartId == Guid.Empty)
        {
            throw new ArgumentException("Cart ID cannot be empty.", nameof(cartId));
        }

        await _cartRepository.DeleteCartAsync(cartId);
    }
    
    public async Task ArchiveCartAsync(Guid cartId)
    {
        var cart = await _cartRepository.GetCartByIdAsync(cartId);
        if (cart == null || cart.Status != CartStatus.Active)
        {
            throw new InvalidOperationException("Cannot archive a non-active cart.");
        }

        cart.Status = CartStatus.Archived;
        cart.LastModifiedAt = DateTime.UtcNow;

        await _cartRepository.UpdateCartAsync(cart);
    }
    
    public async Task HandleAbandonedCartsAsync(TimeSpan inactivityPeriod)
    {
        if (inactivityPeriod.TotalSeconds < 0)
        {
            throw new ArgumentException("Inactivity period cannot be negative.", nameof(inactivityPeriod));
        }
        
        var inactiveCarts = await _cartRepository.GetCartsByStatusAndInactivityAsync(CartStatus.Active, inactivityPeriod);
    
        foreach (var cart in inactiveCarts)
        {
            cart.Status = CartStatus.Abandoned;
            cart.LastModifiedAt = DateTime.UtcNow;
            await _cartRepository.UpdateCartAsync(cart);
        }
    }

    public async Task<CartDto?> RecoverAbandonedCartAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        }
        
        var currentActiveCart = await _cartRepository.GetActiveCartByUserIdAsync(userId);
        if (currentActiveCart != null)
        {
            throw new InvalidOperationException("Cannot recover abandoned cart. There is an active cart.");
        }
        
        var abandonedCart = await _cartRepository.GetLatestAbandonedCartByUserIdAsync(userId);
        if (abandonedCart == null)
        {
            throw new InvalidOperationException("Cannot recover abandoned cart. There is no abandoned cart.");
        }

        abandonedCart.Status = CartStatus.Active;
        abandonedCart.LastModifiedAt = DateTime.UtcNow;

        await _cartRepository.UpdateCartAsync(abandonedCart);

        var cartItems = await GetCartItemsByCartIdAsync(abandonedCart.Id);
        return StoreEntityToDtoMapper.MapToCartDto(abandonedCart, cartItems);
    }

    public async Task<bool> IsCartRelevantAsync(Guid cartId)
    {
        if (cartId == Guid.Empty)
        {
            throw new ArgumentException("Cart ID cannot be empty.", nameof(cartId));
        }
        
        var cart = await _cartRepository.GetCartByIdAsync(cartId);
        if (cart == null || cart.Status != CartStatus.Active)
        {
            throw new InvalidOperationException("Cannot check if cart is relevant. Cart is not active.");
        }
        
        var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(cartId);
        var cartItemsList = cartItems.ToList();
        if (!cartItemsList.Any())
        {
            return true;
        }
        
        List<Guid> checkedBundleIds = [];

        foreach (var cartItem in cartItemsList)
        {
            if (cartItem.BundleId != null)
            {
                if (!checkedBundleIds.Contains(cartItem.BundleId.Value))
                {
                    var bundle = await _bundleRepository.GetByIdAsync(cartItem.BundleId.Value);
                    if (bundle == null)
                    {
                        throw new InvalidOperationException("Bundle not found.");
                    }

                    var bundleCartItems = cartItemsList.Where(ci => ci.BundleId == cartItem.BundleId).ToList();
                    var bundleCartItemsTotalPrice = bundleCartItems.Sum(ci => ci.Price);
                    
                    var bundleProductIds = bundle.BundleProducts.Select(bp => bp.ProductId).ToList();
                    var cartProductIds = cartItemsList.Select(ci => ci.ProductId).ToList();

                    if (bundleCartItems.Count != bundle.BundleProducts.Count
                        || _priceCalculator.ArePricesDifferent(bundle.TotalPrice, bundleCartItemsTotalPrice) 
                        || !bundleProductIds.ToHashSet().SetEquals(cartProductIds.ToHashSet()))
                    {
                        return false;
                    }
                
                    checkedBundleIds.Add(cartItem.BundleId.Value);
                }
            }
            else
            {
                var productDiscounts =
                    await _productDiscountRepository.GetProductDiscountsByProductIdAsync(cartItem.ProductId);
                var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
                if (product == null)
                {
                    throw new InvalidOperationException("Product not found.");
                }
                if (_priceCalculator.ArePricesDifferent(cartItem.Price, _priceCalculator.ApplyDiscount(product.Price, productDiscounts)))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public async Task<CartDto?> UpdateCartAsync(Guid cartId)
    {
        if (cartId == Guid.Empty)
        {
            throw new ArgumentException("Cart ID cannot be empty.", nameof(cartId));
        }
        
        var cart = await _cartRepository.GetCartByIdAsync(cartId);
        if (cart == null || cart.Status != CartStatus.Active)
        {
            throw new InvalidOperationException("Cannot update a non-active cart.");
        }
        
        var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(cartId);
        var cartItemsList = cartItems.ToList();
        if (!cartItemsList.Any())
        {
            return StoreEntityToDtoMapper.MapToCartDto(cart, new List<CartItemDto>());
        }
        
        List<Guid> checkedBundleIds = [];

        foreach (var cartItem in cartItemsList)
        {
            if (cartItem.BundleId != null)
            {
                if (!checkedBundleIds.Contains(cartItem.BundleId.Value))
                {
                    var bundle = await _bundleRepository.GetByIdAsync(cartItem.BundleId.Value);
                    if (bundle == null)
                    {
                        throw new InvalidOperationException("Bundle not found.");
                    }

                    var bundleCartItems = cartItemsList.Where(ci => ci.BundleId == cartItem.BundleId).ToList();
                    var bundleCartItemsTotalPrice = bundleCartItems.Sum(ci => ci.Price);

                    var bundleProductIds = bundle.BundleProducts.Select(bp => bp.ProductId).ToList();
                    var cartProductIds = bundleCartItems.Select(ci => ci.ProductId).ToList();
                    
                    if (bundle.ExpiresAt < DateTime.UtcNow 
                        || !bundle.IsActive
                        || bundleCartItems.Count != bundle.BundleProducts.Count
                        || !bundleProductIds.ToHashSet().SetEquals(cartProductIds.ToHashSet()))
                    {
                        foreach (var item in bundleCartItems)
                        {
                            item.Price = await GetDiscountedPriceAsync(item.ProductId);
                            item.BundleId = null;
                            await _cartItemRepository.UpdateCartItemAsync(item);
                        }
                    }
                    else if (_priceCalculator.ArePricesDifferent(bundle.TotalPrice, bundleCartItemsTotalPrice))
                    {
                        if (!_priceCalculator.ArePricesDifferent(bundle.TotalPrice, bundle.BundleProducts.Sum(bp => bp.DistributedPrice)))
                        {
                            foreach (var item in bundleCartItems)
                            {
                                item.Price = bundle.BundleProducts.First(bp => bp.ProductId == item.ProductId).DistributedPrice;
                                await _cartItemRepository.UpdateCartItemAsync(item);
                            }
                        }
                        else
                        {
                            var bundleProducts = _bundleProductRepository.DistributeBundlePrice(bundle.BundleProducts.ToList(), bundle.TotalPrice);
                            foreach (var product in bundleProducts)
                            {
                                await _bundleProductRepository.UpdateAsync(product);
                            }
                            foreach (var item in bundleCartItems)
                            {
                                item.Price = bundleProducts.First(bp => bp.ProductId == item.ProductId).DistributedPrice;
                                await _cartItemRepository.UpdateCartItemAsync(item);
                            }
                        }
                    }
                
                    checkedBundleIds.Add(cartItem.BundleId.Value);
                }
            }
            else
            {
                var discountedPrice = await GetDiscountedPriceAsync(cartItem.ProductId);
                if (_priceCalculator.ArePricesDifferent(cartItem.Price, discountedPrice))
                {
                    cartItem.Price = discountedPrice;
                    await _cartItemRepository.UpdateCartItemAsync(cartItem);
                }
            }
        }
        
        cart.TotalAmount = await _priceCalculator.CalculateTotalAsync(cartItemsList, _productDiscountRepository);
        cart.LastModifiedAt = DateTime.UtcNow;
        var updatedCart = await _cartRepository.UpdateCartAsync(cart);
        if (updatedCart == null)
        {
            throw new InvalidOperationException("Cannot update cart.");
        }
        
        var cartItemsDtos = await GetCartItemsByCartIdAsync(cartId);
        
        return StoreEntityToDtoMapper.MapToCartDto(updatedCart, cartItemsDtos);
    }
    
    private async Task<decimal> GetDiscountedPriceAsync(Guid productId)
    {
        var product = await _productRepository.GetByIdAsync(productId)
                      ?? throw new InvalidOperationException("Product not found.");
        var discounts = await _productDiscountRepository.GetProductDiscountsByProductIdAsync(productId);
        return _priceCalculator.ApplyDiscount(product.Price, discounts.ToList());
    }

    // ---------------- Cart Item Operations --------------------

    public async Task<CartItemDto?> GetCartItemByIdAsync(Guid cartItemId)
    {
        if (cartItemId == Guid.Empty)
        {
            throw new ArgumentException("CartItem ID cannot be empty.", nameof(cartItemId));
        }

        var cartItem = await _cartItemRepository.GetCartItemByIdAsync(cartItemId);
        if (cartItem == null) return null;
        
        if (cartItem.Product == null)
        {
            throw new InvalidOperationException("Product not found.");
        }
        
        var fullLogoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(cartItem.ProductId);
        decimal? discountValue = _priceCalculator.ArePricesDifferent(cartItem.Product.Price, cartItem.Price) ? cartItem.Product.Price - cartItem.Price : null;
        DateTime? discountExpirationDate = null;
        
        if (cartItem.BundleId != null)
        {
            var bundle = await _bundleRepository.GetByIdAsync(cartItem.BundleId.Value);
            if (bundle == null)
            {
                throw new InvalidOperationException("Bundle not found.");
            }
            discountExpirationDate = bundle.ExpiresAt;
        }
        else
        {
            var productDiscounts = await _productDiscountRepository.GetProductDiscountsByProductIdAsync(cartItem.ProductId);
            var now = DateTime.UtcNow;
            var activeDiscounts = productDiscounts
                .Where(pd =>
                    (!pd.Discount.StartDate.HasValue || pd.Discount.StartDate <= now) &&
                    (!pd.Discount.EndDate.HasValue || pd.Discount.EndDate >= now))
                .ToList();

            var endDates = activeDiscounts.Select(pd => pd.Discount.EndDate).ToList();
            if (endDates.Any())
            {
                discountExpirationDate = endDates.Min();
            }
        }
        
        return StoreEntityToDtoMapper.MapToCartItemDto(cartItem, fullLogoUrl, discountValue, discountExpirationDate);
    }

    public async Task<List<CartItemDto>> GetCartItemsByCartIdAsync(Guid cartId)
    {
        if (cartId == Guid.Empty)
        {
            throw new ArgumentException("Cart ID cannot be empty.", nameof(cartId));
        }

        var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(cartId);
        
        var result = new List<CartItemDto>();

        foreach (var cartItem in cartItems)
        {
            if (cartItem.Product == null)
            {
                throw new InvalidOperationException($"Product not found for cart item with ID: {cartItem.Id}");
            }

            var fullLogoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(cartItem.ProductId);
            decimal? discountValue = _priceCalculator.ArePricesDifferent(cartItem.Product.Price, cartItem.Price)
                ? cartItem.Product.Price - cartItem.Price
                : null;

            DateTime? discountExpirationDate = null;

            if (cartItem.BundleId != null)
            {
                var bundle = await _bundleRepository.GetByIdAsync(cartItem.BundleId.Value);
                if (bundle == null)
                {
                    throw new InvalidOperationException($"Bundle not found for cart item with ID: {cartItem.Id}");
                }

                discountExpirationDate = bundle.ExpiresAt;
            }
            else
            {
                var productDiscounts = await _productDiscountRepository.GetProductDiscountsByProductIdAsync(cartItem.ProductId);
                var now = DateTime.UtcNow;

                var activeDiscounts = productDiscounts
                    .Where(pd =>
                        (!pd.Discount.StartDate.HasValue || pd.Discount.StartDate <= now) &&
                        (!pd.Discount.EndDate.HasValue || pd.Discount.EndDate >= now))
                    .ToList();

                var endDates = activeDiscounts
                    .Select(pd => pd.Discount.EndDate)
                    .Where(d => d.HasValue)
                    .ToList();

                if (endDates.Any())
                {
                    discountExpirationDate = endDates.Min();
                }
            }

            var dto = StoreEntityToDtoMapper.MapToCartItemDto(cartItem, fullLogoUrl, discountValue, discountExpirationDate);
            result.Add(dto);
        }

        return result;
    }

    public async Task<CartItemDto?> AddCartItemAsync(CartItemCreateRequest request)
    {
        if (!StoreRequestsValidator.ValidateCartItemCreateRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Errors: {errors}", nameof(request));
        }
        
        var cart = await _cartRepository.GetCartByIdAsync(request.CartId);
        if (cart == null || cart.Status != CartStatus.Active)
        {
            throw new InvalidOperationException("Cannot add items to a non-active cart.");
        }
        
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
        {
            throw new InvalidOperationException("Product not found.");
        }
        
        var productDiscounts = await _productDiscountRepository.GetProductDiscountsByProductIdAsync(request.ProductId);
        var productDiscountsList = productDiscounts.ToList();
        var now = DateTime.UtcNow;
        var activeDiscounts = productDiscountsList
            .Where(pd =>
                (!pd.Discount.StartDate.HasValue || pd.Discount.StartDate <= now) &&
                (!pd.Discount.EndDate.HasValue || pd.Discount.EndDate >= now))
            .ToList();

        var cartItem = new CartItem
        {
            Id = Guid.NewGuid(),
            CartId = request.CartId,
            ProductId = request.ProductId,
            BundleId = null,
            Price = _priceCalculator.ApplyDiscount(product.Price, activeDiscounts)
        };

        var addedCartItem = await _cartItemRepository.AddCartItemAsync(cartItem);
        if (addedCartItem == null) return null;
        
        await RecalculateCartTotalAsync(request.CartId);
        
        if (addedCartItem.Product == null)
        {
            throw new InvalidOperationException("Product not found.");
        }
        
        var fullLogoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(addedCartItem.ProductId);
        decimal? discountValue = _priceCalculator.ArePricesDifferent(addedCartItem.Product.Price, addedCartItem.Price) ? addedCartItem.Product.Price - addedCartItem.Price : null;
        DateTime? discountExpirationDate = null;
        
        var endDates = activeDiscounts.Select(pd => pd.Discount.EndDate).ToList();
        if (endDates.Any())
        {
            discountExpirationDate = endDates.Min();
        }
        
        return StoreEntityToDtoMapper.MapToCartItemDto(addedCartItem, fullLogoUrl, discountValue, discountExpirationDate);
    }

    public async Task RemoveCartItemAsync(Guid cartItemId)
    {
        if (cartItemId == Guid.Empty)
        {
            throw new ArgumentException("CartItem ID cannot be empty.", nameof(cartItemId));
        }

        var cartItem = await _cartItemRepository.GetCartItemByIdAsync(cartItemId);
        if (cartItem == null)
        {
            throw new KeyNotFoundException($"CartItem with ID {cartItemId} not found.");
        }

        await _cartItemRepository.DeleteCartItemAsync(cartItemId);

        await RecalculateCartTotalAsync(cartItem.CartId);
    }
    
    public async Task RecalculateCartTotalAsync(Guid cartId)
    {
        var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(cartId);
        var cartItemsList = cartItems.ToList();
        if (!cartItemsList.Any())
        {
            return;
        }

        decimal totalPrice = await _priceCalculator.CalculateTotalAsync(cartItemsList, _productDiscountRepository);

        var cart = await _cartRepository.GetCartByIdAsync(cartId);
        if (cart != null)
        {
            cart.TotalAmount = totalPrice;
            cart.LastModifiedAt = DateTime.UtcNow;
            await _cartRepository.UpdateCartAsync(cart);
        }
    }

    public async Task<List<CartItemDto>> AddBundleToCartAsync(CartAddBundleRequest request)
    {
        if (!StoreRequestsValidator.ValidateCartAddBundleRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Errors: {errors}", nameof(request));
        }
        var bundle = await _bundleRepository.GetByIdAsync(request.BundleId);
        if (bundle == null)
        {
            throw new InvalidOperationException("Bundle not found.");
        }
        if (bundle.BundleProducts == null || bundle.BundleProducts.Count == 0)
        {
            throw new InvalidOperationException("Bundle has no products.");
        }
        
        List<CartItem> addedCartItems = [];
        foreach (var bundleProduct in bundle.BundleProducts)
        {
            var product = await _productRepository.GetByIdAsync(bundleProduct.ProductId);
            if (product == null)
            {
                throw new InvalidOperationException("Product not found.");
            }
            
            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),
                CartId = request.CartId,
                ProductId = bundleProduct.ProductId,
                BundleId = request.BundleId,
                Price = bundleProduct.DistributedPrice
            };

            var addedCartItem = await _cartItemRepository.AddCartItemAsync(cartItem);
            if (addedCartItem == null)
            {
                foreach (var item in addedCartItems)
                {
                    await _cartItemRepository.DeleteCartItemAsync(item.Id);
                }
                
                throw new InvalidOperationException("Cannot add bundle to cart.");
            }
            addedCartItems.Add(addedCartItem);
        }
        
        await RecalculateCartTotalAsync(request.CartId);
        
        var result = new List<CartItemDto>();

        foreach (var cartItem in addedCartItems)
        {
            var fullLogoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(cartItem.ProductId);
            decimal? discountValue = _priceCalculator.ArePricesDifferent(cartItem.Product.Price, cartItem.Price)
                ? cartItem.Product.Price - cartItem.Price
                : null;
            DateTime? discountExpirationDate =  bundle.ExpiresAt;

            var dto = StoreEntityToDtoMapper.MapToCartItemDto(cartItem, fullLogoUrl, discountValue, discountExpirationDate);
            result.Add(dto);
        }

        return result;
    }
}