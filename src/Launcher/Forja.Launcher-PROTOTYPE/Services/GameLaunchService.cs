namespace Forja.Launcher.Services;

public sealed class GameLaunchService
{
    private readonly ApiService _apiService;
    private Process? _gameProcess;
    private Stopwatch? _stopwatch;
    private InstalledGameModel? _currentGame;

    public InstalledGameModel? CurrentGame
    {
        get => _currentGame;
        private set
        {
            if (_currentGame == value) return;
            _currentGame = value;
            CurrentGameChanged?.Invoke(this, _currentGame);
        }
    }
    
    public bool IsRunning => _gameProcess is { HasExited: false };
    
    public event EventHandler<InstalledGameModel?>? CurrentGameChanged;
    
    public event EventHandler<bool>? GameRunningChanged;

    public GameLaunchService(ApiService apiService)
    {
        _apiService = apiService;
    }
    
    public void LaunchGame(InstalledGameModel game)
    {
        if (IsRunning)
            return;

        var installPath = game.InstallPath;
        if (string.IsNullOrEmpty(installPath))
            throw new ArgumentNullException(nameof(installPath), "No install path provided.");

        var executablePath = FindExecutablePath(installPath);
        if (string.IsNullOrEmpty(executablePath) || !File.Exists(executablePath))
            return;

        CurrentGame = game;
        GameRunningChanged?.Invoke(this, true);

        _gameProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = executablePath,
                UseShellExecute = false,
                WorkingDirectory = Path.GetDirectoryName(executablePath)
            },
            EnableRaisingEvents = true
        };
        _gameProcess.Exited += OnGameExited;
        _gameProcess.Start();

        _stopwatch = Stopwatch.StartNew();
    }

    public async Task StopGameAsync()
    {
        if (_gameProcess is { HasExited: false })
        {
            _gameProcess.Kill();
            try
            {
                await Task.Run(() => _gameProcess.WaitForExit());
            }
            catch { /* ignore exceptions */ }
        }
        CleanupAfterGameExit();
    }

    private void OnGameExited(object? sender, EventArgs e)
    {
        try
        {
            _stopwatch?.Stop();
            if (_stopwatch != null && CurrentGame != null)
            {
                _apiService.ReportPlayedTimeAsync(CurrentGame.Id, _stopwatch.Elapsed)
                    .ConfigureAwait(false);
            }
        }
        catch { /* ignore exceptions */ }

        CleanupAfterGameExit();
    }

    private void CleanupAfterGameExit()
    {
        if (_gameProcess != null)
        {
            _gameProcess.Exited -= OnGameExited;
            _gameProcess.Dispose();
            _gameProcess = null;
        }
        _stopwatch = null;

        GameRunningChanged?.Invoke(this, false);
        CurrentGame = null;
    }
    
    private string? FindExecutablePath(string installPath)
    {
        if (OperatingSystem.IsWindows())
        {
            var exeFiles = Directory.GetFiles(installPath, "*.exe", SearchOption.AllDirectories);
            return exeFiles.FirstOrDefault();
        }

        if (!OperatingSystem.IsLinux() && !OperatingSystem.IsMacOS()) return null;
        if (OperatingSystem.IsMacOS())
        {
            var appBundles = Directory.GetDirectories(installPath, "*.app", SearchOption.AllDirectories);
            foreach (var bundle in appBundles)
            {
                var macOsPath = Path.Combine(bundle, "Contents", "MacOS");
                if (!Directory.Exists(macOsPath)) continue;
                var macExecutables = Directory.GetFiles(macOsPath)
                    .Where(IsExecutable);
                var executables = macExecutables.ToList();
                if (executables.Count != 0)
                    return executables.First();
            }
        }

        var files = Directory.GetFiles(installPath, "*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var extension = Path.GetExtension(file).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) && IsExecutable(file) && file.Contains("TestGame"))
            {
                return file;
            }
        }

        return null;
    }

    private static bool IsExecutable(string path)
    {
        try
        {
            var fileInfo = new UnixFileInfo(path);
            return fileInfo.Exists &&
                   (fileInfo.FileAccessPermissions & FileAccessPermissions.UserExecute) != 0;
        }
        catch
        {
            return false;
        }
    }
}