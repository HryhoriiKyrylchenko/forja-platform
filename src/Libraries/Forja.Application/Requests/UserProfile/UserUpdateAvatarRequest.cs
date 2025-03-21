namespace Forja.Application.Requests.UserProfile;

public class UserUpdateAvatarRequest
{
    public Guid Id { get; set; }
    public string AvatarUrl { get; set; } = string.Empty;
}