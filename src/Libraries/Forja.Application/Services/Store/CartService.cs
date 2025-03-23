namespace Forja.Application.Services.Store;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IProductDiscountRepository _productDiscountRepository;
    private readonly IPriceCalculator _priceCalculator;
    private readonly IProductRepository _productRepository;

    public CartService(ICartRepository cartRepository, 
        ICartItemRepository cartItemRepository,
        IProductDiscountRepository productDiscountRepository,
        IPriceCalculator priceCalculator,
        IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
        _productDiscountRepository = productDiscountRepository;
        _priceCalculator = priceCalculator;
        _productRepository = productRepository;
    }

    // ---------------- Cart Operations --------------------

    public async Task<Cart?> GetCartByIdAsync(Guid cartId)
    {
        if (cartId == Guid.Empty)
        {
            throw new ArgumentException("Cart ID cannot be empty.", nameof(cartId));
        }

        return await _cartRepository.GetCartByIdAsync(cartId);
    }

    public async Task<IEnumerable<Cart>> GetCartsByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        }

        return await _cartRepository.GetCartsByUserIdAsync(userId);
    }

    public async Task<Cart> GetOrCreateActiveCartAsync(CartCreateRequest request)
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
        }

        return activeCart;
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

    public async Task<Cart?> RecoverAbandonedCartAsync(Guid userId)
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
            return null;
        }

        abandonedCart.Status = CartStatus.Active;
        abandonedCart.LastModifiedAt = DateTime.UtcNow;

        await _cartRepository.UpdateCartAsync(abandonedCart);

        return abandonedCart;
    }

    // ---------------- Cart Item Operations --------------------

    public async Task<CartItem?> GetCartItemByIdAsync(Guid cartItemId)
    {
        if (cartItemId == Guid.Empty)
        {
            throw new ArgumentException("CartItem ID cannot be empty.", nameof(cartItemId));
        }

        return await _cartItemRepository.GetCartItemByIdAsync(cartItemId);
    }

    public async Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(Guid cartId)
    {
        if (cartId == Guid.Empty)
        {
            throw new ArgumentException("Cart ID cannot be empty.", nameof(cartId));
        }

        return await _cartItemRepository.GetCartItemsByCartIdAsync(cartId);
    }

    public async Task AddCartItemAsync(CartItemCreateRequest request)
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

        var cartItem = new CartItem
        {
            Id = Guid.NewGuid(),
            CartId = request.CartId,
            ProductId = request.ProductId,
            Price = product.Price
        };

        await _cartItemRepository.AddCartItemAsync(cartItem);

        await RecalculateCartTotalAsync(request.CartId);
    }

    public async Task UpdateCartItemAsync(CartItemUpdateRequest request)
    {
        if (!StoreRequestsValidator.ValidateCartItemUpdateRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Errors: {errors}", nameof(request));
        }

        var cartItem = await _cartItemRepository.GetCartItemByIdAsync(request.Id);
        if (cartItem == null)
        {
            throw new KeyNotFoundException($"CartItem with ID {request.Id} not found.");
        }
        
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
        {
            throw new InvalidOperationException("Product not found.");
        }

        cartItem.CartId = request.CartId;
        cartItem.ProductId = request.ProductId;
        cartItem.Price = product.Price;

        await _cartItemRepository.UpdateCartItemAsync(cartItem);

        await RecalculateCartTotalAsync(request.CartId);
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
}