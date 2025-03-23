namespace Forja.Application.Requests.Store;

public class PaymentUpdateSatusRequest
{
    [Required]
    public Guid Id { get; set; }
}