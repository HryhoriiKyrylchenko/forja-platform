namespace Forja.Application.Services.Store;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    // ---------------- Payment Operations ----------------

    public async Task<Payment?> GetPaymentByIdAsync(Guid paymentId)
    {
        if (paymentId == Guid.Empty)
        {
            throw new ArgumentException("Payment ID cannot be empty.", nameof(paymentId));
        }

        return await _paymentRepository.GetPaymentByIdAsync(paymentId);
    }

    public async Task<Payment?> GetPaymentByTransactionIdAsync(string transactionId)
    {
        if (string.IsNullOrWhiteSpace(transactionId))
        {
            throw new ArgumentException("Transaction ID cannot be null or empty.", nameof(transactionId));
        }

        return await _paymentRepository.GetPaymentByTransactionIdAsync(transactionId);
    }

    public async Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(Guid orderId)
    {
        if (orderId == Guid.Empty)
        {
            throw new ArgumentException("Order ID cannot be empty.", nameof(orderId));
        }

        return await _paymentRepository.GetPaymentsByOrderIdAsync(orderId);
    }

    public async Task AddPaymentAsync(PaymentCreateRequest request)
    {
        if (!StoreRequestsValidator.ValidatePaymentCreateRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Errors: {errors}", nameof(request));
        }

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = request.OrderId,
            PaymentMethod = request.PaymentMethod,
            Amount = request.Amount,
            ExternalPaymentId = request.ExternalPaymentId,
            ProviderName = request.ProviderName,
            ProviderResponse = request.ProviderResponse,
            PaymentStatus = request.PaymentStatus,
            PaymentDate = DateTime.UtcNow
        };

        await _paymentRepository.AddPaymentAsync(payment);
    }

    public async Task UpdatePaymentAsync(PaymentUpdateRequest request)
    {
        if (!StoreRequestsValidator.ValidatePaymentUpdateRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Errors: {errors}", nameof(request));
        }

        var payment = await _paymentRepository.GetPaymentByIdAsync(request.Id);
        if (payment == null)
        {
            throw new KeyNotFoundException($"Payment with ID {request.Id} not found.");
        }

        payment.OrderId = request.OrderId;
        payment.PaymentMethod = request.PaymentMethod;
        payment.Amount = request.Amount;
        payment.PaymentDate = request.PaymentDate != default ? request.PaymentDate : payment.PaymentDate;
        payment.ExternalPaymentId = request.ExternalPaymentId;
        payment.ProviderName = request.ProviderName;
        payment.ProviderResponse = request.ProviderResponse;
        payment.PaymentStatus = request.PaymentStatus;

        await _paymentRepository.UpdatePaymentAsync(payment);
    }

    public async Task DeletePaymentAsync(Guid paymentId)
    {
        if (paymentId == Guid.Empty)
        {
            throw new ArgumentException("Payment ID cannot be empty.", nameof(paymentId));
        }

        await _paymentRepository.DeletePaymentAsync(paymentId);
    }
}