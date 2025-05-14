namespace Forja.Launcher.Services;

public class GameLaunchService
{
    private readonly ApiService _apiService;
    
    private Process? _gameProcess;
    public Stopwatch? Stopwatch { get; private set; }
    public InstalledGameModel? CurrentGame { get; private set; }
    
    private bool _isRunning;
    public bool IsRunning
    {
        get => _isRunning;
        set
        {
            if (_isRunning != value)
            {
                _isRunning = value;
                RaiseGameRunningChanged(_isRunning);
            }
        }
    }
    
    public event EventHandler<bool>? GameRunningChanged;

     private void RaiseGameRunningChanged(bool isRunning)
     {
         GameRunningChanged?.Invoke(this, isRunning);
     }
    
     public GameLaunchService(ApiService apiService)
     {
         _apiService = apiService;
         Debug.WriteLine($"GameLaunchService created. HashCode: {GetHashCode()}");
     }
     
     public void LaunchGame(InstalledGameModel game)
     {
         if (IsRunning)
             return;
         
         var installPath = game.InstallPath;
         if (string.IsNullOrEmpty(installPath))
         {
             throw new ArgumentNullException(nameof(installPath), "No install path provided.");
         }
         var executablePath = FindExecutablePath(installPath);
         
         if (string.IsNullOrEmpty(executablePath) || !File.Exists(executablePath)) return;
     
         CurrentGame = game;
     
         _gameProcess = new Process();
         _gameProcess.StartInfo = new ProcessStartInfo
         {
             FileName = executablePath,
             UseShellExecute = false,
             WorkingDirectory = Path.GetDirectoryName(executablePath)
         };
     
         _gameProcess.Exited += OnGameExited;
         _gameProcess.EnableRaisingEvents = true;
     
         _gameProcess.Start();
     
         Stopwatch = Stopwatch.StartNew();
         IsRunning = true;
     }
     
     public async Task StopGameAsync()
     {
         Debug.WriteLine($"Attempting to stop game. IsRunning: {IsRunning}, Process Null: {_gameProcess == null}, HasExited: {_gameProcess?.HasExited}");
         
         if (_gameProcess is { HasExited: false })
         {
             _gameProcess.Kill();
     
             try
             {
                 await Task.Run(() => _gameProcess.WaitForExit());
             }
             catch (Exception ex)
             {
                 Debug.WriteLine("Error waiting for process to exit: " + ex.Message);
             }
         }
     
         CleanupAfterGameExit();
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
         }
         catch (Exception exception)
         {
             Debug.WriteLine(exception);
         }
     }
     
     private void CleanupAfterGameExit()
     {
         if (_gameProcess != null)
         {
             _gameProcess.Exited -= OnGameExited;
             _gameProcess.Dispose();
             _gameProcess = null;
         }
     
         Stopwatch = null;
         CurrentGame = null;
         IsRunning = false;
     
         RaiseGameRunningChanged(false);
     }
    
    private string? FindExecutablePath(string installPath)
    {
        if (OperatingSystem.IsWindows())
        {
            var exeFiles = Directory.GetFiles(installPath, "*.exe", SearchOption.AllDirectories);
            return exeFiles.FirstOrDefault();
        }

        if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
        {
            if (OperatingSystem.IsMacOS())
            {
                var appBundles = Directory.GetDirectories(installPath, "*.app", SearchOption.AllDirectories);
                foreach (var bundle in appBundles)
                {
                    var macOsPath = Path.Combine(bundle, "Contents", "MacOS");
                    if (Directory.Exists(macOsPath))
                    {
                        var macExecutables = Directory.GetFiles(macOsPath)
                            .Where(IsExecutable);
                        var executables = macExecutables.ToList();
                        if (executables.Any())
                            return executables.First();
                    }
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
            
            // return files
            //     .Where(file => string.IsNullOrEmpty(Path.GetExtension(file)) && IsExecutable(file))
            //     .OrderByDescending(File.GetLastWriteTime) 
            //     .FirstOrDefault();
        }

        return null;
    }

    private bool IsExecutable(string path)
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