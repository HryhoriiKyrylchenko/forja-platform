namespace Forja.Launcher.Services;

public class GameLaunchService
{
    private Process? _gameProcess;
    public Stopwatch? Stopwatch { get; private set;}
    public InstalledGameModel? CurrentGame { get; private set; }
    public bool IsRunning => _gameProcess is { HasExited: false };
    
    private readonly ApiService _apiService;
    
    public event EventHandler<bool>? GameRunningChanged;

    private void RaiseGameRunningChanged(bool isRunning)
    {
        GameRunningChanged?.Invoke(this, isRunning);
    }
    
    public GameLaunchService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public void LaunchGame(InstalledGameModel game)
    {
        var installPath = game.InstallPath;
        if (string.IsNullOrEmpty(installPath))
        {
            throw new ArgumentNullException(nameof(installPath), "No install path provided.");
        }
        var executablePath = FindExecutablePath(installPath);
        
        if (string.IsNullOrEmpty(executablePath) || !File.Exists(executablePath)) return;

        if (IsRunning)
            return;

        CurrentGame = game;

        _gameProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = executablePath,
                UseShellExecute = true,
                WorkingDirectory = Path.GetDirectoryName(executablePath)
            },
            EnableRaisingEvents = true
        };

        _gameProcess.Exited += OnGameExited;
        _gameProcess.Start();
        
        RaiseGameRunningChanged(true);

        Stopwatch?.Reset();
        Stopwatch = Stopwatch.StartNew();
    }

    public void StopGame()
    {
        if (_gameProcess is { HasExited: false })
        {
            _gameProcess.Kill(true);
            _gameProcess.WaitForExit();
            
            RaiseGameRunningChanged(false);
        }
    }

    private async void OnGameExited(object? sender, EventArgs e)
    {
        try
        {
            Stopwatch?.Stop();
            
            if (Stopwatch != null && CurrentGame != null)
            {
                await _apiService.ReportPlayedTimeAsync(CurrentGame.Id, Stopwatch.Elapsed);
            }
            
            if (_gameProcess != null)
            {
                _gameProcess.Exited -= OnGameExited;
                _gameProcess.Dispose();
                _gameProcess = null;
            }
            RaiseGameRunningChanged(false);
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception);
        }
    }
    
    private string? FindExecutablePath(string installPath)
    {
        if (OperatingSystem.IsWindows())
        {
            // Look for .exe files
            var exeFiles = Directory.GetFiles(installPath, "*.exe", SearchOption.AllDirectories);
            return exeFiles.FirstOrDefault();
        }
        else if (OperatingSystem.IsLinux())
        {
            // Look for executable files (no extension or .sh)
            var files = Directory.GetFiles(installPath, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                if ((fileInfo.Attributes & FileAttributes.Directory) == 0)
                {
                    if (file.EndsWith(".sh") || IsExecutable(file))
                    {
                        return file;
                    }
                }
            }
        }
        else if (OperatingSystem.IsMacOS())
        {
            // Look for .app bundles
            var appBundles = Directory.GetDirectories(installPath, "*.app", SearchOption.AllDirectories);
            if (appBundles.Length > 0)
            {
                // Inside .app bundle: Contents/MacOS/{ExecutableName}
                var executable = Path.Combine(appBundles[0], "Contents", "MacOS");
                if (Directory.Exists(executable))
                {
                    var filesMac = Directory.GetFiles(executable);
                    if (filesMac.Length > 0)
                        return filesMac[0];  // Usually only one main binary
                }
            }

            // Fallback: same as Linux
            var files = Directory.GetFiles(installPath, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (IsExecutable(file))
                {
                    return file;
                }
            }
        }

        return null;
    }

    private bool IsExecutable(string path)
    {
        try
        {
            var fileInfo = new Mono.Unix.UnixFileInfo(path);
            return fileInfo.FileAccessPermissions.HasFlag(Mono.Unix.FileAccessPermissions.UserExecute);
        }
        catch
        {
            return false;
        }
    }
}