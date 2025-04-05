namespace Forja.Domain.Entities.Games;

[Table("Games", Schema = "games")]
public class Game : Product
{
    public string? SystemRequirements { get; set; }

    public TimeSpan? TimePlayed { get; set; }

    public virtual ICollection<GameAddon> GameAddons { get; set; } = [];

    public virtual ICollection<GameTag> GameTags { get; set; } = [];

    public virtual ICollection<UserLibraryGame> UserLibraryGames { get; set; } = [];

    public virtual ICollection<GameMechanic> GameMechanics { get; set; } = [];
    
    public virtual ICollection<GameVersion> GameVersions { get; set; } = [];
    
    public virtual ICollection<GamePatch> GamePatches { get; set; } = [];
    
    public virtual ICollection<Achievement> Achievements { get; set; } = [];
}