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
    private readonly IUserLibraryService _userLibraryService;

    public CartService(ICartRepository cartRepository, 
        ICartItemRepository cartItemRepository,
        IProductDiscountRepository productDiscountRepository,
        IPriceCalculator priceCalculator,
        IProductRepository productRepository,
        IBundleRepository bundleRepository,
        IBundleProductRepository bundleProductRepository,
        IFileManagerService fileManagerService,
        IUserLibraryService userLibraryService)
    {
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
        _productDiscountRepository = productDiscountRepository;
        _priceCalculator = priceCalculator;
        _productRepository = productRepository;
        _bundleRepository = bundleRepository;
        _bundleProductRepository = bundleProductRepository;
        _fileManagerService = fileManagerService;
        _userLibraryService = userLibraryService;
    }

    // ---------------- Cart Operations --------------------

    ///<inheritdoc/>
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

    ///<inheritdoc/>
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

    ///<inheritdoc/>
    public async Task<CartDto> GetOrCreateActiveCartAsync(CartCreateRequest request)
    {
        if (!StoreRequestsValidator.ValidateCartCreateRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Errors: {errors}", nameof(request));
        }
        
        var activeCart = await _cartRepository.GetActiveCartByUserIdAsync(request.UserId);

        if (activeCart == null)
        {
            var newCart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Status = CartStatus.Active,
                CreatedAt = DateTime.UtcNow
            };
            
            activeCart = await _cartRepository.AddCartAsync(newCart);
            if (activeCart == null)
            {
                throw new InvalidOperationException("Cannot create cart.");
            }
            
            return StoreEntityToDtoMapper.MapToCartDto(activeCart, new List<CartItemDto>());
        }

        if (!await IsCartRelevantAsync(activeCart.Id))
        {
            var updatedCart = await UpdateCartAsync(activeCart.Id);
            if (updatedCart == null)
            {
                throw new InvalidOperationException("Cannot update cart.");
            }
            return updatedCart;
        }
            
        var cartItems = await GetCartItemsByCartIdAsync(activeCart.Id);
        return StoreEntityToDtoMapper.MapToCartDto(activeCart, cartItems);
    }

    ///<inheritdoc/>
    public async Task RemoveCartAsync(Guid cartId)
    {
        if (cartId == Guid.Empty)
        {
            throw new ArgumentException("Cart ID cannot be empty.", nameof(cartId));
        }

        await _cartRepository.DeleteCartAsync(cartId);
    }
    
    ///<inheritdoc/>
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
    
    ///<inheritdoc/>
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

    ///<inheritdoc/>
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

    ///<inheritdoc/>
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

        var cartItemsList = cart.CartItems.ToList();
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

        if (cart.TotalAmount != cartItemsList.Sum(ci => ci.Price))
        {
            return false;
        }

        return true;
    }

    ///<inheritdoc/>
    public async Task<CartDto?> UpdateCartAsync(Guid cartId)
    {
        if (cartId == Guid.Empty)
            throw new ArgumentException("Cart ID cannot be empty.", nameof(cartId));

        var cart = await ValidateAndGetCartAsync(cartId);
        var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(cartId);
        var cartItemsList = cartItems.ToList();
        if (!cartItemsList.Any())
            return StoreEntityToDtoMapper.MapToCartDto(cart, new List<CartItemDto>());

        var checkedBundleIds = new HashSet<Guid>();

        foreach (var item in cartItemsList)
        {
            if (item.BundleId != null)
            {
                if (!checkedBundleIds.Contains((Guid)item.BundleId))
                {
                    await HandleBundleCartItemAsync((Guid)item.BundleId, cartItemsList);
                    checkedBundleIds.Add((Guid)item.BundleId);
                }
            }
            else
            {
                await HandleRegularCartItemAsync(item);
            }
        }

        await UpdateCartTotalAsync(cart, cartItemsList);

        var updatedCart = await _cartRepository.UpdateCartAsync(cart)
                          ?? throw new InvalidOperationException("Cannot update cart.");

        var cartItemsDtos = await GetCartItemsByCartIdAsync(cartId);
        return StoreEntityToDtoMapper.MapToCartDto(updatedCart, cartItemsDtos);
    }

    private async Task<Cart> ValidateAndGetCartAsync(Guid cartId)
    {
        var cart = await _cartRepository.GetCartByIdAsync(cartId);
        if (cart == null || cart.Status != CartStatus.Active)
            throw new InvalidOperationException("Cannot update a non-active cart.");

        return cart;
    }

    private async Task HandleRegularCartItemAsync(CartItem cartItem)
    {
        var discountedPrice = await GetDiscountedPriceAsync(cartItem.ProductId);
        if (_priceCalculator.ArePricesDifferent(cartItem.Price, discountedPrice))
        {
            cartItem.Price = discountedPrice;
            await _cartItemRepository.UpdateCartItemAsync(cartItem);
        }
    }
    
    private async Task HandleBundleCartItemAsync(Guid bundleId, List<CartItem> allCartItems)
    {
        var bundle = await _bundleRepository.GetByIdAsync(bundleId)
                     ?? throw new InvalidOperationException($"Bundle {bundleId} not found.");

        var bundleCartItems = allCartItems.Where(ci => ci.BundleId == bundleId).ToList();
        var bundleProductIds = bundle.BundleProducts.Select(bp => bp.ProductId).ToHashSet();
        var cartProductIds = bundleCartItems.Select(ci => ci.ProductId).ToHashSet();

        bool isInvalid = 
            bundle.ExpiresAt < DateTime.UtcNow ||
            !bundle.IsActive ||
            bundleProductIds.Count != bundleCartItems.Count ||
            !bundleProductIds.SetEquals(cartProductIds);

        if (isInvalid)
        {
            foreach (var item in bundleCartItems)
            {
                item.Price = await GetDiscountedPriceAsync(item.ProductId);
                item.BundleId = null;
                await _cartItemRepository.UpdateCartItemAsync(item);
            }
            return;
        }

        var bundleCartTotal = bundleCartItems.Sum(ci => ci.Price);
        var bundleDistributedSum = bundle.BundleProducts.Sum(bp => bp.DistributedPrice);

        if (_priceCalculator.ArePricesDifferent(bundle.TotalPrice, bundleCartTotal))
        {
            if (!_priceCalculator.ArePricesDifferent(bundle.TotalPrice, bundleDistributedSum))
            {
                foreach (var item in bundleCartItems)
                {
                    var matching = bundle.BundleProducts.FirstOrDefault(bp => bp.ProductId == item.ProductId);
                    if (matching != null)
                    {
                        item.Price = matching.DistributedPrice;
                        await _cartItemRepository.UpdateCartItemAsync(item);
                    }
                }
            }
            else
            {
                var redistributed = await _bundleProductRepository
                    .DistributeBundlePrice(bundle.BundleProducts.ToList(), bundle.TotalPrice);

                foreach (var updated in redistributed)
                    await _bundleProductRepository.UpdateAsync(updated);

                foreach (var item in bundleCartItems)
                {
                    var match = redistributed.FirstOrDefault(bp => bp.ProductId == item.ProductId);
                    if (match != null)
                    {
                        item.Price = match.DistributedPrice;
                        var updatedItem = await _cartItemRepository.UpdateCartItemAsync(item);
                        if (updatedItem == null)
                            throw new InvalidOperationException("Cannot update cart item.");
                    }
                }
            }
        }
    }

    private async Task UpdateCartTotalAsync(Cart cart, List<CartItem> items)
    {
        cart.TotalAmount = await _priceCalculator.CalculateTotalAsync(items, _productDiscountRepository);
        cart.LastModifiedAt = DateTime.UtcNow;
    }
    
    private async Task<decimal> GetDiscountedPriceAsync(Guid productId)
    {
        var product = await _productRepository.GetByIdAsync(productId)
                      ?? throw new InvalidOperationException("Product not found.");
        var discounts = await _productDiscountRepository.GetProductDiscountsByProductIdAsync(productId);
        return _priceCalculator.ApplyDiscount(product.Price, discounts.ToList());
    }

    // ---------------- Cart Item Operations --------------------

    ///<inheritdoc/>
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

    ///<inheritdoc/>
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

    ///<inheritdoc/>
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
        
        var userLibraryProducts = await _userLibraryService.GetUserLibraryProductIdsByUserIdAsync(cart.UserId);
        if (userLibraryProducts.Any(p => p == request.ProductId))
        {
            throw new InvalidOperationException("User already has this product in their library.");
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

    ///<inheritdoc/>
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
    
    ///<inheritdoc/>
    public async Task RecalculateCartTotalAsync(Guid cartId)
    {
        var cart = await _cartRepository.GetCartByIdAsync(cartId);
        if (cart == null || cart.Status != CartStatus.Active)
        {
            throw new InvalidOperationException("Cannot recalculate a non-active cart.");
        }
        
        var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(cartId);
        var cartItemsList = cartItems.ToList();
        if (cartItemsList.Count == 0)
        {
            cart.TotalAmount = 0;
            await _cartRepository.UpdateCartAsync(cart);
            return;
        }

        decimal totalPrice = await _priceCalculator.CalculateTotalAsync(cartItemsList, _productDiscountRepository);
        
        cart.TotalAmount = totalPrice;
        cart.LastModifiedAt = DateTime.UtcNow;
        await _cartRepository.UpdateCartAsync(cart);
    }

    ///<inheritdoc/>
    public async Task<List<CartItemDto>> AddBundleToCartAsync(CartAddBundleRequest request)
    {
        if (!StoreRequestsValidator.ValidateCartAddBundleRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Errors: {errors}", nameof(request));
        }
        
        var cart = await _cartRepository.GetCartByIdAsync(request.CartId);
        if (cart == null || cart.Status != CartStatus.Active)
        {
            throw new InvalidOperationException("Cannot add items to a non-active cart.");
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
        
        var userLibraryProducts = await _userLibraryService.GetUserLibraryProductIdsByUserIdAsync(cart.UserId);
        if (userLibraryProducts.Any(p => bundle.BundleProducts.Any(bp => bp.ProductId == p)))
        {
            throw new InvalidOperationException($"Couldn't add bundle to the cart. User already has product from bundle in their library.");
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