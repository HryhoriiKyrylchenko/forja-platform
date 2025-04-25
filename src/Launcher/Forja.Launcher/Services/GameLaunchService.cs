namespace Forja.Launcher.Services;

public class GameLaunchService
{
    private Process? _gameProcess;
    public Stopwatch? Stopwatch { get; private set;}
    public Game? CurrentGame { get; private set; }

    public event Action<Game, TimeSpan>? GameExited;
    public TimeSpan Elapsed => Stopwatch?.Elapsed ?? TimeSpan.Zero;

    private bool IsRunning => _gameProcess is { HasExited: false };

    public void LaunchGame(Game game)
    {
        if (!File.Exists(game.ExecutablePath)) return;

        if (IsRunning)
            return;

        CurrentGame = game;

        _gameProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = game.ExecutablePath,
                UseShellExecute = true
            },
            EnableRaisingEvents = true
        };

        _gameProcess.Exited += OnGameExited;
        _gameProcess.Start();

        Stopwatch = Stopwatch.StartNew();
    }

    public void StopGame()
    {
        if (_gameProcess is { HasExited: false })
        {
            _gameProcess.Kill(true);
            _gameProcess.WaitForExit();
        }
    }

    private void OnGameExited(object? sender, EventArgs e)
    {
        Stopwatch?.Stop();

        if (Stopwatch != null && CurrentGame != null)
        {
            GameExited?.Invoke(CurrentGame, Stopwatch.Elapsed);
        }

        _gameProcess?.Dispose();
        _gameProcess = null;
    }
}