namespace Forja.Application.Mapping;

public class StoreEntityToDtoMapper
{
    public static CartDto MapToCartDto(Cart cart)
    {
        return new CartDto
        {
            Id = cart.Id,
            UserId = cart.UserId,
            TotalAmount = cart.TotalAmount,
            Status = cart.Status.ToString(),
            CreatedAt = cart.CreatedAt,
            LastModifiedAt = cart.LastModifiedAt,
            CartItems = cart.CartItems.Select(MapToCartItemDto).ToList()
        };
    }

    public static CartItemDto MapToCartItemDto(CartItem cartItem)
    {
        return new CartItemDto
        {
            Id = cartItem.Id,
            CartId = cartItem.CartId,
            ProductId = cartItem.ProductId,
            Price = cartItem.Price
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
            Status = order.Status
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