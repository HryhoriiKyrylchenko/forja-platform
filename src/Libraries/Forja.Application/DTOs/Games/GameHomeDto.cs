namespace Forja.Application.DTOs.Games;

public class GameHomeDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public string Developer { get; set; } = string.Empty;
    public List<string> Genres { get; set; } = [];
    public List<string> Tags { get; set; } = [];
    public List<string> Images { get; set; } = [];
}