namespace Forja.Application.Mapping;

public class StoreEntityToDtoMapper
{
    public static CartDto MapToCartDto(Cart cart, List<CartItemDto> cartItems)
    {
        return new CartDto
        {
            Id = cart.Id,
            UserId = cart.UserId,
            TotalAmount = cart.TotalAmount,
            Status = cart.Status.ToString(),
            CreatedAt = cart.CreatedAt,
            LastModifiedAt = cart.LastModifiedAt,
            CartItems = cartItems
        };
    }

    public static CartItemDto MapToCartItemDto(CartItem cartItem, string fullLogoUrl, decimal? totalDiscountValue = null, DateTime? discountExpirationDate = null)
    {
        return new CartItemDto
        {
            Id = cartItem.Id,
            CartId = cartItem.CartId,
            ProductId = cartItem.ProductId,
            BundleId = cartItem.BundleId,
            Title = cartItem.Product.Title,
            ShortDescription = cartItem.Product.ShortDescription,
            LogoUrl = fullLogoUrl,
            OriginalPrice = cartItem.Product.Price,
            TotalDiscountValue = totalDiscountValue,
            DiscountExpirationDate = discountExpirationDate,
            TotalPrice = cartItem.Price,
            IsAddon = cartItem.Product is GameAddon
        };
    }

    public static DiscountDto MapToDiscountDto(Discount discount)
    {
        return new DiscountDto
        {
            Id = discount.Id,
            Name = discount.Name,
            DiscountType = discount.DiscountType.ToString(),
            DiscountValue = discount.DiscountValue,
            StartDate = discount.StartDate,
            EndDate = discount.EndDate
        };
    }

    public static OrderDto MapToOrderDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            CartId = order.CartId,
            OrderDate = order.OrderDate,
            Status = order.Status.ToString()
        };
    }

    public static PaymentDto MapToPaymentDto(Payment payment)
    {
        return new PaymentDto
        {
            Id = payment.Id,
            OrderId = payment.OrderId,
            PaymentMethod = payment.PaymentMethod.ToString(),
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            ExternalPaymentId = payment.ExternalPaymentId,
            ProviderName = payment.ProviderName.ToString(),
            ProviderResponse = payment.ProviderResponse.ToString(),
            IsRefunded = payment.IsRefunded
        };
    }

    public static ProductDiscountDto MapToProductDiscountDto(ProductDiscount productDiscount)
    {
        return new ProductDiscountDto
        {
            Id = productDiscount.Id,
            ProductId = productDiscount.ProductId,
            DiscountId = productDiscount.DiscountId
        };
    }
}