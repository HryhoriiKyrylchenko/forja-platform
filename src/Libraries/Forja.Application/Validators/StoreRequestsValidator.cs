using Forja.Application.Requests.Store;

namespace Forja.Application.Validators;

public static class StoreRequestsValidator
{
    public static bool ValidateCartCreateRequest(CartCreateRequest request, out string? error)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.UserId == Guid.Empty)
        {
            error = "User ID cannot be empty.";
            return false;
        }

        error = null;
        return true;
    }
    
    public static bool ValidateCartItemCreateRequest(CartItemCreateRequest request, out string? error)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.CartId == Guid.Empty)
        {
            error = "Cart ID cannot be empty.";
            return false;
        }

        if (request.ProductId == Guid.Empty)
        {
            error = "Product ID cannot be empty.";
            return false;
        }

        error = null;
        return true;
    }
    
    public static bool ValidateCartItemUpdateRequest(CartItemUpdateRequest request, out string? error)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Id == Guid.Empty)
        {
            error = "Cart item ID cannot be empty.";
            return false;
        }

        if (request.CartId == Guid.Empty)
        {
            error = "Cart ID cannot be empty.";
            return false;
        }

        if (request.ProductId == Guid.Empty)
        {
            error = "Product ID cannot be empty.";
            return false;
        }

        error = null;
        return true;
    }

    public static bool ValidateDiscountCreateRequest(DiscountCreateRequest request, out string? error)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 50)
        {
            error = "Discount name cannot be empty or exceed 50 characters.";
            return false;
        }

        if (request.DiscountValue < 0)
        {
            error = "Discount value cannot be negative.";
            return false;
        }

        if (request is { StartDate: not null, EndDate: not null } && request.StartDate > request.EndDate)
        {
            error = "Start date cannot be later than the end date.";
            return false;
        }

        error = null;
        return true;
    }

    public static bool ValidateDiscountUpdateRequest(DiscountUpdateRequest request, out string? error)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Id == Guid.Empty)
        {
            error = "Discount ID cannot be empty.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 50)
        {
            error = "Discount name cannot be empty or exceed 50 characters.";
            return false;
        }

        if (request.DiscountValue < 0)
        {
            error = "Discount value cannot be negative.";
            return false;
        }

        if (request is { StartDate: not null, EndDate: not null } && request.StartDate > request.EndDate)
        {
            error = "Start date cannot be later than the end date.";
            return false;
        }

        error = null;
        return true;
    }

    public static bool ValidateOrderCreateRequest(OrderCreateRequest request, out string? error)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.CartId == Guid.Empty)
        {
            error = "Cart ID cannot be empty.";
            return false;
        }

        error = null;
        return true;
    }

    public static bool ValidateOrderUpdateRequest(OrderUpdateRequest request, out string? error)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Id == Guid.Empty)
        {
            error = "Order ID cannot be empty.";
            return false;
        }

        error = null;
        return true;
    }

    public static bool ValidatePaymentCreateRequest(PaymentCreateRequest request, out string? error)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.OrderId == Guid.Empty)
        {
            error = "Order ID cannot be empty.";
            return false;
        }

        if (request.Amount < 0)
        {
            error = "Payment amount cannot be negative.";
            return false;
        }

        error = null;
        return true;
    }

    public static bool ValidatePaymentUpdateRequest(PaymentUpdateRequest request, out string? error)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Id == Guid.Empty)
        {
            error = "Payment ID cannot be empty.";
            return false;
        }

        if (request.OrderId == Guid.Empty)
        {
            error = "Order ID cannot be empty.";
            return false;
        }

        if (request.Amount < 0)
        {
            error = "Payment amount cannot be negative.";
            return false;
        }

        error = null;
        return true;
    }

    public static bool ValidateProductDiscountCreateRequest(ProductDiscountCreateRequest request, out string? error)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.ProductId == Guid.Empty)
        {
            error = "Product ID cannot be empty.";
            return false;
        }

        if (request.DiscountId == Guid.Empty)
        {
            error = "Discount ID cannot be empty.";
            return false;
        }

        error = null;
        return true;
    }
    
    public static bool ValidateProductDiscountUpdateRequest(ProductDiscountUpdateRequest request, out string? error)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Id == Guid.Empty)
        {
            error = "Product discount ID cannot be empty.";
            return false;
        }

        if (request.ProductId == Guid.Empty)
        {
            error = "Product ID cannot be empty.";
            return false;
        }

        if (request.DiscountId == Guid.Empty)
        {
            error = "Discount ID cannot be empty.";
            return false;
        }

        error = null;
        return true;
    }
}