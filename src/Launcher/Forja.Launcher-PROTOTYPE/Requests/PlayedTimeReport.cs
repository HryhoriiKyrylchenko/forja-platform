namespace Forja.Launcher.Requests;

public class PlayedTimeReport
{
    public string LibraryGameId { get; set; } = string.Empty;
    public int SecondsPlayed { get; set; }
}