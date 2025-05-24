namespace Forja.Application.Requests.Storage;

public class ProfileHatVariantGetByIdRequest
{
    [Required]
    public short ProfileHatVariantId { get; set; }
}