namespace Forja.Application.Requests.Storage;

public class ProfileHatVariantGetByIdRequest
{
    [Required]
    [Range(1, 5)]
    public short ProfileHatVariantId { get; set; }
}