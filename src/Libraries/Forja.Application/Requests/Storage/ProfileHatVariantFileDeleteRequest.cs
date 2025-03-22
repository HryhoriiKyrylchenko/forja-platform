namespace Forja.Application.Requests.Storage;

public class ProfileHatVariantFileDeleteRequest
{
    [Required]
    [Range(1, 5)]
    public short ProfileHatVariantId { get; set; }
}