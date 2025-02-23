namespace Forja.Application.DTOs.UserProfile;

public class UserLibraryAddonDto
{
    public Guid Id { get; set; }
    public UserLibraryGame UserLibraryGame { get; set; } = null!;
    public GameAddon GameAddon { get; set; } = null!;
    public DateTime PurchaseDate { get; set; }
}