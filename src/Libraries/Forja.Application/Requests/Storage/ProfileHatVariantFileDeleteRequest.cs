namespace Forja.Application.Requests.Storage;

public class ProfileHatVariantFileDeleteRequest
{
    [Required]
    public short ProfileHatVariantId { get; set; }
}