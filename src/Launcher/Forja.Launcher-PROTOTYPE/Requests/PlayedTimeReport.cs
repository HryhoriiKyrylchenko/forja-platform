namespace Forja.Launcher.Requests;

public class PlayedTimeReport
{
    public Guid LibraryGameId { get; set; }
    public TimeSpan TimePlayed { get; set; }
}