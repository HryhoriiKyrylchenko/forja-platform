namespace Forja.Application.Requests.UserProfile;

public class UserUpdateProfileHatVariantRequest
{
    [Required]
    public Guid UserId { get; set; }
    
    [Range(1, 5)]
    public short Variant { get; set; }
}