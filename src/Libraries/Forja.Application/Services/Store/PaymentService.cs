using Forja.Infrastructure.Services;

namespace Forja.Application.Services.Store;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ITestPaymentService _testPaymentService;
    private readonly IOrderService _iOrderService;

    public PaymentService(IPaymentRepository paymentRepository,
        ITestPaymentService testPaymentService,
        IOrderService orderService)
    {
        _paymentRepository = paymentRepository;
        _testPaymentService = testPaymentService;
        _iOrderService = orderService;
    }

    public async Task<PaymentDto?> GetPaymentByIdAsync(Guid paymentId)
    {
        if (paymentId == Guid.Empty)
        {
            throw new ArgumentException("Payment ID cannot be empty.", nameof(paymentId));
        }

        var result = await _paymentRepository.GetPaymentByIdAsync(paymentId);
        
        return result == null ? null : StoreEntityToDtoMapper.MapToPaymentDto(result);
    }

    public async Task<PaymentDto?> GetPaymentByTransactionIdAsync(string transactionId)
    {
        if (string.IsNullOrWhiteSpace(transactionId))
        {
            throw new ArgumentException("Transaction ID cannot be null or empty.", nameof(transactionId));
        }

        var result = await _paymentRepository.GetPaymentByTransactionIdAsync(transactionId);
        
        return result == null ? null : StoreEntityToDtoMapper.MapToPaymentDto(result);
    }

    public async Task<IEnumerable<PaymentDto>> GetPaymentsByOrderIdAsync(Guid orderId)
    {
        if (orderId == Guid.Empty)
        {
            throw new ArgumentException("Order ID cannot be empty.", nameof(orderId));
        }

        var payments = await _paymentRepository.GetPaymentsByOrderIdAsync(orderId);
        
        return payments.Select(StoreEntityToDtoMapper.MapToPaymentDto);
    }

    public async Task DeletePaymentAsync(Guid paymentId)
    {
        if (paymentId == Guid.Empty)
        {
            throw new ArgumentException("Payment ID cannot be empty.", nameof(paymentId));
        }

        await _paymentRepository.DeletePaymentAsync(paymentId);
    }
    
    public async Task<string> ExecutePaymentAsync(PaymentRequest request)
    {
        if (!StoreRequestsValidator.ValidatePaymentRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Errors: {errors}", nameof(request));
        }

        string transactionId;
        PaymentStatus providerResponse;

        switch (request.PaymentMethod)
        {
            case PaymentMethod.Custom:
                transactionId = await _testPaymentService.ProcessPaymentAsync(
                    request.Amount,
                    request.Currency,
                    request.PaymentMethodToken
                );
                providerResponse = await _testPaymentService.GetPaymentStatusAsync(transactionId);
                break;
            default:
                throw new ArgumentException("Invalid payment method.", nameof(request));
        }
        
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = request.OrderId,
            PaymentMethod = request.PaymentMethod,
            Amount = request.Amount,
            ExternalPaymentId = transactionId,
            ProviderName = request.ProviderName,
            ProviderResponse = providerResponse,
            PaymentDate = DateTime.UtcNow
        };

        var addedPayment = await _paymentRepository.AddPaymentAsync(payment);
        if (addedPayment == null)
        {
            throw new InvalidOperationException("Failed to add payment to the database.");
        }

        await ProcessPaymentStatus(addedPayment);

        return transactionId;
    }
    
    public async Task<PaymentDto?> UpdatePaymentStatusAsync(PaymentUpdateSatusRequest satusRequest)
    {
        if (!StoreRequestsValidator.ValidatePaymentUpdateRequest(satusRequest, out var errors))
        {
            throw new ArgumentException($"Invalid request. Errors: {errors}", nameof(satusRequest));
        }

        var payment = await _paymentRepository.GetPaymentByIdAsync(satusRequest.Id);
        if (payment == null)
        {
            throw new KeyNotFoundException($"Payment with ID {satusRequest.Id} not found.");
        }
        
        var newStatus = await _testPaymentService.GetPaymentStatusAsync(payment.ExternalPaymentId);

        payment.ProviderResponse = newStatus;

        var updatedPayment = await _paymentRepository.UpdatePaymentAsync(payment);
        if (updatedPayment == null)
        {
            throw new InvalidOperationException("Failed to update payment status.");
        }
        await ProcessPaymentStatus(updatedPayment);
        
        return StoreEntityToDtoMapper.MapToPaymentDto(updatedPayment);
    }

    public async Task<bool> RefundPaymentAsync(Guid paymentId)
    {
        var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
        if (payment == null)
        {
            throw new KeyNotFoundException($"Payment with ID {paymentId} not found.");
        }

        var result = await _testPaymentService.RefundPaymentAsync(payment.ExternalPaymentId, payment.Amount);

        if (result)
        {
            payment.ProviderResponse = PaymentStatus.Refunded;
            var refundedPayment = await _paymentRepository.UpdatePaymentAsync(payment);
            if (refundedPayment == null)
            {
                throw new InvalidOperationException("Failed to update payment status.");
            }
            await ProcessPaymentStatus(refundedPayment);
        }

        return result;
    }

    private async Task ProcessPaymentStatus(Payment payment)
    {
        if (payment == null)
        {
            throw new ArgumentNullException(nameof(payment));
        }

        var order = await _iOrderService.GetOrderByIdAsync(payment.OrderId);
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {payment.OrderId} not found.");
        }


        switch (payment.ProviderResponse)
        {
            case PaymentStatus.Pending:
                if (order.Status.Contains(OrderStatus.Canceled.ToString()))
                {
                    throw new InvalidOperationException("Order is already canceled.");
                }

                var pendingOrder = await _iOrderService.UpdateOrderStatusAsync(new OrderUpdateRequest
                {
                    Id = payment.OrderId,
                    Status = OrderStatus.Pending
                });
                if (pendingOrder == null)
                {
                    throw new KeyNotFoundException($"Failed to update the order with ID {payment.OrderId}");
                }

                break;
            case PaymentStatus.Failed:
                if (order.Status.Contains(OrderStatus.Completed.ToString())
                    || order.Status.Contains(OrderStatus.Canceled.ToString()))
                {
                    throw new InvalidOperationException("Order is already completed.");
                }

                var faidelOrder = await _iOrderService.UpdateOrderStatusAsync(new OrderUpdateRequest
                {
                    Id = payment.OrderId,
                    Status = OrderStatus.Failed
                });
                if (faidelOrder == null)
                {
                    throw new KeyNotFoundException($"Failed to update the order with ID {payment.OrderId}");
                }

                break;
            case PaymentStatus.Refunded:
                if (!order.Status.Contains(OrderStatus.Completed.ToString()))
                {
                    throw new InvalidOperationException("Order is not completed.");
                }

                var refundedOrder = await _iOrderService.UpdateOrderStatusAsync(new OrderUpdateRequest
                {
                    Id = payment.OrderId,
                    Status = OrderStatus.Canceled
                });
                if (refundedOrder == null)
                {
                    throw new KeyNotFoundException($"Failed to update the order with ID {payment.OrderId}");
                }

                break;
            case PaymentStatus.Completed:
                if (order.Status.Contains(OrderStatus.Canceled.ToString()))
                {
                    throw new InvalidOperationException("Order is already canceled.");
                }

                var completedOrder = await _iOrderService.UpdateOrderStatusAsync(new OrderUpdateRequest
                {
                    Id = payment.OrderId,
                    Status = OrderStatus.Canceled
                });
                if (completedOrder == null)
                {
                    throw new KeyNotFoundException($"Failed to update the order with ID {payment.OrderId}");
                }

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(payment.ProviderResponse), payment.ProviderResponse, null);
        }
    }
}