namespace Forja.Application.Requests.Store;

public class OrderUpdateRequest
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public OrderPaymentStatus PaymentStatus { get; set; }
}