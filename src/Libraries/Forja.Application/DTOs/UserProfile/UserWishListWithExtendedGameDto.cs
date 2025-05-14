namespace Forja.Application.DTOs.UserProfile;

public class UserWishListWithExtendedGameDto
{
    public Guid Id { get; set; }
    public GameWishListDto Game { get; set; } = null!;
}