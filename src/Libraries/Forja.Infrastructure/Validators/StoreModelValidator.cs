namespace Forja.Infrastructure.Validators;

public static class StoreModelValidator
{
    /// <summary>
    /// Validates a Cart model for correctness.
    /// </summary>
    /// <param name="cartModel">The Cart model to validate.</param>
    /// <param name="errorMessage">An output parameter containing the error message if validation fails.</param>
    /// <returns>True if the model is valid; otherwise, false.</returns>
    public static bool ValidateCartModel(Cart cartModel, out string errorMessage)
    {
        if (cartModel == null)
            throw new ArgumentNullException(nameof(cartModel));

        if (cartModel.Id == Guid.Empty)
        {
            errorMessage = "Cart ID cannot be empty.";
            return false;
        }

        if (cartModel.UserId == Guid.Empty)
        {
            errorMessage = "User ID cannot be empty.";
            return false;
        }

        if (cartModel.TotalAmount < 0)
        {
            errorMessage = "Total amount cannot be negative.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates a CartItem model for correctness.
    /// </summary>
    /// <param name="cartItemModel">The CartItem model to validate.</param>
    /// <param name="errorMessage">An output parameter containing the error message if validation fails.</param>
    /// <returns>True if the model is valid; otherwise, false.</returns>
    public static bool ValidateCartItemModel(CartItem cartItemModel, out string errorMessage)
    {
        if (cartItemModel == null)
        {
            throw new ArgumentNullException(nameof(cartItemModel));
        }

        if (cartItemModel.Id == Guid.Empty)
        {
            errorMessage = "Cart item ID cannot be empty.";
            return false;
        }

        if (cartItemModel.CartId == Guid.Empty)
        {
            errorMessage = "Cart ID cannot be empty.";
            return false;
        }

        if (cartItemModel.ProductId == Guid.Empty)
        {
            errorMessage = "Product ID cannot be empty.";
            return false;
        }

        if (cartItemModel.Price <= 0)
        {
            errorMessage = "Price cannot be negative or zero.";
            return false;
        }
        
        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates a Discount model for correctness.
    /// </summary>
    /// <param name="discountModel">The Discount model to validate.</param>
    /// <param name="errorMessage">An output parameter containing the error message if validation fails.</param>
    /// <returns>True if the model is valid; otherwise, false.</returns>
    public static bool ValidateDiscountModel(Discount discountModel, out string errorMessage)
    {
        if (discountModel == null)
        {
            throw new ArgumentNullException(nameof(discountModel));
        }
        
        if (discountModel.Id == Guid.Empty)
        {
            errorMessage = "Discount ID cannot be empty.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(discountModel.Name) || discountModel.Name.Length > 50)
        {
            errorMessage = "Discount name must be between 1 and 50 characters long.";
            return false;
        }

        if (discountModel.DiscountValue <= 0)
        {
            errorMessage = "Discount value must be greater than zero.";
            return false;
        }
        
        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates an Order model for correctness.
    /// </summary>
    /// <param name="orderModel">The Order model to validate.</param>
    /// <param name="errorMessage">An output parameter containing the error message if validation fails.</param>
    /// <returns>True if the model is valid; otherwise, false.</returns>
    public static bool ValidateOrderModel(Order orderModel, out string errorMessage)
    {
        if (orderModel == null)
        {
            throw new ArgumentNullException(nameof(orderModel));
        }
        
        if (orderModel.Id == Guid.Empty)
        {
            errorMessage = "Order ID cannot be empty.";
            return false;
        }
        
        if (orderModel.UserId == Guid.Empty)
        {
            errorMessage = "User ID cannot be empty.";
            return false;
        }

        if (orderModel.OrderDate > DateTime.UtcNow)
        {
            errorMessage = "Order date cannot be in the future.";
            return false;
        }
        
        if (orderModel.TotalAmount <= 0)
        {
            errorMessage = "Total amount must be greater than zero.";
            return false;
        }
        
        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates an OrderItem model for correctness.
    /// </summary>
    /// <param name="orderItemModel">The OrderItem model to validate.</param>
    /// <param name="errorMessage">An output parameter containing the error message if validation fails.</param>
    /// <returns>True if the model is valid; otherwise, false.</returns>
    public static bool ValidateOrderItemModel(OrderItem orderItemModel, out string errorMessage)
    {
        if (orderItemModel == null)
        {
            throw new ArgumentNullException(nameof(orderItemModel));
        }

        if (orderItemModel.Id == Guid.Empty)
        {
            errorMessage = "Order item ID cannot be empty.";
            return false;
        }
        
        if (orderItemModel.OrderId == Guid.Empty)
        {
            errorMessage = "Order ID cannot be empty.";
            return false;
        }
        
        if (orderItemModel.ProductId == Guid.Empty)
        {
            errorMessage = "Product ID cannot be empty.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates a Payment model for correctness.
    /// </summary>
    /// <param name="paymentModel">The Payment model to validate.</param>
    /// <param name="errorMessage">An output parameter containing the error message if validation fails.</param>
    /// <returns>True if the model is valid; otherwise, false.</returns>
    public static bool ValidatePaymentModel(Payment paymentModel, out string errorMessage)
    {
        if (paymentModel == null)
        {
            throw new ArgumentNullException(nameof(paymentModel));
        }
        
        if (paymentModel.Id == Guid.Empty)
        {
            errorMessage = "Payment ID cannot be empty.";
            return false;
        }

        if (paymentModel.OrderId == Guid.Empty)
        {
            errorMessage = "Order ID cannot be empty.";
            return false;
        }

        if (paymentModel.Amount <= 0)
        {
            errorMessage = "Amount must be greater than zero.";
            return false;
        }
        
        if (paymentModel.PaymentDate > DateTime.UtcNow)
        {
            errorMessage = "Payment date cannot be in the future.";
            return false;
        }
        
        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates a Product Discount model for correctness.
    /// </summary>
    /// <param name="productDiscountModel">The Product Discount model to validate.</param>
    /// <param name="errorMessage">An output parameter containing the error message if validation fails.</param>
    /// <returns>True if the model is valid; otherwise, false.</returns>
    public static bool ValidateProductDiscountModel(ProductDiscount productDiscountModel, out string errorMessage)
    {
        if (productDiscountModel == null)
        {
            throw new ArgumentNullException(nameof(productDiscountModel));
        }
        
        if (productDiscountModel.Id == Guid.Empty)
        {
            errorMessage = "Product discount ID cannot be empty.";
            return false;
        }
        
        if (productDiscountModel.ProductId == Guid.Empty)
        {
            errorMessage = "Product ID cannot be empty.";
            return false;
        }
        
        if (productDiscountModel.DiscountId == Guid.Empty)
        {
            errorMessage = "Discount ID cannot be empty.";
            return false;
        }
        
        errorMessage = string.Empty;
        return true;
    }
}