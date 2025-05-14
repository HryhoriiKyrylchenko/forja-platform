namespace Forja.Launcher.Requests;

public class PlayedTimeReport
{
    public Guid Id { get; set; }
    public TimeSpan TimePlayed { get; set; }
}