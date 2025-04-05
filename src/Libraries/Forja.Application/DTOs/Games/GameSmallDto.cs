namespace Forja.Application.DTOs.Games;

public class GameSmallDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
}