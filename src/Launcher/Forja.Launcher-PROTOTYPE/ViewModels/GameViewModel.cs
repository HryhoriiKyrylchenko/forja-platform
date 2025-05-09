namespace Forja.Launcher.ViewModels;

public class GameViewModel : ReactiveObject
{
    public LibraryGameModel? Game { get; }
    public InstalledGameModel? LocalData { get; set; }

    private readonly GameInstallationService _installationService;
    private readonly GameLaunchService _gameLaunchService;

    public ReactiveCommand<Unit, Unit> CheckForUpdateCommand { get; }
    public ReactiveCommand<Unit, Unit> RepairGameCommand { get; }
    public ReactiveCommand<Unit, Unit> InstallOrUpdateCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteGameCommand { get; }
    public ReactiveCommand<Unit, Unit> PlayCommand { get; }
    public ReactiveCommand<Unit, Unit> StopCommand { get; }
    
    private double _progress;
    public double Progress
    {
        get => _progress;
        set => this.RaiseAndSetIfChanged(ref _progress, value);
    }
    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set => this.RaiseAndSetIfChanged(ref _isSelected, value);
    }
    private bool _isInstalled;
    public bool IsInstalled
    {
        get => _isInstalled;
        set => this.RaiseAndSetIfChanged(ref _isInstalled, value);
    }
    private bool _isUpdated;
    public bool IsUpdated
    {
        get => _isUpdated;
        set => this.RaiseAndSetIfChanged(ref _isUpdated, value);
    }
    private bool _isRunning;
    public bool IsRunning
    {
        get => _isRunning;
        set => this.RaiseAndSetIfChanged(ref _isRunning, value);
    }
    
    private Bitmap? _logoBitmap;
    public Bitmap? LogoBitmap
    {
        get => _logoBitmap;
        private set
        {
            _logoBitmap?.Dispose();
            this.RaiseAndSetIfChanged(ref _logoBitmap, value);
        }
    }
    
    public bool IsUnavailable { get; set; }

    private string _statusMessage = "Idle";
    public string StatusMessage
    {
        get => _statusMessage;
        set => this.RaiseAndSetIfChanged(ref _statusMessage, value);
    }

    public GameViewModel(LibraryGameModel? game, ApiService apiService, InstalledGameModel? localData = null)
    {
        Game = game;
        LocalData = localData;
        _installationService = new GameInstallationService(apiService);
        _gameLaunchService = new GameLaunchService(apiService);

        CheckForUpdateCommand = ReactiveCommand.CreateFromTask(CheckForUpdateAsync);
        RepairGameCommand = ReactiveCommand.CreateFromTask(RepairGameAsync);
        InstallOrUpdateCommand = ReactiveCommand.CreateFromTask(UpdateOrInstallGameAndAddonsAsync);
        DeleteGameCommand = ReactiveCommand.CreateFromTask(DeleteGameAsync);
        PlayCommand = ReactiveCommand.CreateFromTask(PlayGameAsync);
        StopCommand = ReactiveCommand.CreateFromTask(StopGameAsync);
        
        _gameLaunchService.GameRunningChanged += (_, isRunning) =>
        {
            IsRunning = isRunning;
        };
        
        InitializeState();
    }
    
    private void InitializeState()
    {
        IsInstalled = LocalData is { Files.Count: > 0 } && !string.IsNullOrWhiteSpace(LocalData.InstallPath);

        if (IsInstalled)
        {
            var currentPlatform = _installationService.GetCurrentPlatformType();
            var latestVersion = Game?.Versions
                .Where(v => v.Platform == currentPlatform)
                .OrderByDescending(v => Version.Parse(v.Version))
                .FirstOrDefault();

            IsUpdated = latestVersion != null &&
                        string.Equals(LocalData!.InstalledVersion, latestVersion.Version, StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            IsUpdated = false;
        }
    }


    private Task PlayGameAsync()
    {
        if (LocalData == null || _gameLaunchService.IsRunning)
        {
            StatusMessage = "Impossible to play. Game is not installed or already running.";
            return Task.CompletedTask;
        }

        try
        {
            _gameLaunchService.LaunchGame(LocalData);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error starting a game: {ex.Message}");
            StatusMessage = "Error starting a game.";
        }
        return Task.CompletedTask;
    }

    private Task StopGameAsync()
    {
        if (!_gameLaunchService.IsRunning)
        {
            StatusMessage = "Impossible to stop. Game is not running.";
            return Task.CompletedTask;
        }
        try
        {
            _gameLaunchService.StopGame();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error stopping a game: {ex.Message}");
            StatusMessage = "Error stopping a game.";
        }
        return Task.CompletedTask;
    }

    private async Task CheckForUpdateAsync()
    {
        if (LocalData == null)
        {
            StatusMessage = "Impossible to check. Game is not installed.";
            return;
        }
        try
        {
            StatusMessage = "Checking for updates...";

            bool updateNeeded = Game != null && await _installationService.CheckForUpdateAsync(Game, LocalData);
            StatusMessage = updateNeeded ? "Update required." : "Game is up to date.";
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error checking for updates: {ex.Message}");
            StatusMessage = "Error checking for updates.";
        }
    }

    private async Task RepairGameAsync()
    {
        try
        {
            if (LocalData == null)
            {
                StatusMessage = "Impossible to repair. Game is not installed.";
                return;
            }
            StatusMessage = "Verifying files...";

            var currentVersion = GetGameCurrentVersion();
            if (currentVersion == null)
            {
                StatusMessage = "Installed version data not found.";
                return;
            }

            var filesToCheck = currentVersion.Files;
            var addonsFilesToCheck = GetAddonsCurrentVersions();
            if (addonsFilesToCheck.Any())
            {
                filesToCheck.AddRange(addonsFilesToCheck.SelectMany(av => av.Files));
            }
            
            if (filesToCheck.Count == 0)
            {
                StatusMessage = "No file metadata available for verification.";
                return;
            }

            var corruptedFiles = await _installationService.VerifyIntegrityAsync(LocalData, filesToCheck);

            if (corruptedFiles.Any())
            {
                StatusMessage = $"Repairing {corruptedFiles.Count} files...";
                await _installationService.DownloadAndReplaceFilesAsync(corruptedFiles, LocalData.Id, LocalData.InstallPath);
                StatusMessage = "Repair complete.";
            }
            else
            {
                StatusMessage = "All files are valid.";
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error during repair: {ex.Message}");
            StatusMessage = "Error during repair.";
        }
    }
    
    private async Task UpdateOrInstallGameAndAddonsAsync()
    {
        var userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var installPath = LocalData?.InstallPath ?? Path.Combine(userDirectory, "Forja", "games");

        if (Game != null)
        {
            LocalData ??= ModelMapper.MapToInstalledGame(Game, installPath);

            var currentPlatform = _installationService.GetCurrentPlatformType();
            var progress = new Progress<double>(p => Progress = p);

            StatusMessage = "Checking for patch...";
            bool patched = await _installationService.ApplyPatchIfAvailableAsync(LocalData, Game, progress);

            var latestVersion = Game.Versions
                .Where(v => v.Platform == currentPlatform)
                .OrderByDescending(v => Version.Parse(v.Version))
                .FirstOrDefault();

            if (patched)
            {
                StatusMessage = $"Game patched to version {LocalData.InstalledVersion}.";
            }
            else
            {
                StatusMessage = "No patch available. Verifying integrity...";

                if (latestVersion == null)
                {
                    StatusMessage = "No available version for this platform.";
                    return;
                }

                var filesToFix = await _installationService.VerifyIntegrityAsync(LocalData, latestVersion.Files);

                if (filesToFix.Count > 0 && filesToFix.Count < latestVersion.Files.Count)
                {
                    StatusMessage = $"Repairing {filesToFix.Count} files...";
                    await _installationService.DownloadAndReplaceFilesAsync(filesToFix, Game.Id, LocalData.InstallPath);

                    LocalData.InstalledVersion = latestVersion.Version;
                    LocalData.Files = latestVersion.Files.Select(ModelMapper.MapToIbInstalledFileModel).ToList();

                    StatusMessage = $"Game repaired to version {LocalData.InstalledVersion}.";
                }
                else if (filesToFix.Count == latestVersion.Files.Count ||
                         string.IsNullOrEmpty(LocalData.InstalledVersion))
                {
                    StatusMessage = "Too many files missing or corrupted. Performing full install...";
                    await _installationService.DownloadAndInstallFullAsync(Game, LocalData.InstallPath, progress);

                    LocalData.InstalledVersion = latestVersion.Version;
                    LocalData.Files = latestVersion.Files.Select(ModelMapper.MapToIbInstalledFileModel).ToList();

                    StatusMessage = $"Game fully installed to version {LocalData.InstalledVersion}.";
                }
                else
                {
                    StatusMessage = "Game is up to date.";
                }
            }

            if (Game.Addons.Count > 0)
            {
                foreach (var remoteAddon in Game.Addons)
                {
                    var latestAddonVersion = remoteAddon.Versions
                        .Where(v => v.Platform == currentPlatform)
                        .OrderByDescending(v => Version.Parse(v.Version))
                        .FirstOrDefault();

                    if (latestAddonVersion == null)
                    {
                        Debug.WriteLine($"No available version for addon {remoteAddon.Name}.");
                        continue;
                    }

                    var localAddon = LocalData.InstalledAddons.FirstOrDefault(a => a.Id == remoteAddon.Id);

                    if (localAddon == null)
                    {
                        StatusMessage = $"Installing addon {remoteAddon.Name}...";
                        await _installationService.InstallAddonAsync(LocalData, remoteAddon, progress);
                        StatusMessage = $"Addon {remoteAddon.Name} installed.";
                    }
                    else if (!string.Equals(localAddon.InstalledVersion, latestAddonVersion.Version,
                                 StringComparison.OrdinalIgnoreCase))
                    {
                        StatusMessage = $"Updating addon {remoteAddon.Name}...";
                        await _installationService.InstallAddonAsync(LocalData, remoteAddon, progress);
                        StatusMessage = $"Addon {remoteAddon.Name} updated to version {latestAddonVersion.Version}.";
                    }
                    else
                    {
                        Debug.WriteLine($"Addon {remoteAddon.Name} is already up to date.");
                    }
                }
            }

            IsInstalled = LocalData != null;
            IsUpdated = LocalData?.InstalledVersion == latestVersion?.Version;
        }
    }
    
    private async Task DeleteGameAsync()
    {
        if (LocalData == null)
        {
            StatusMessage = "Game is not installed.";
            return;
        }

        try
        {
            StatusMessage = "Deleting game files...";
            var gameFolder = LocalData.InstallPath;

            if (Directory.Exists(gameFolder))
            {
                await Task.Run(() =>
                {
                    try
                    {
                        if (_gameLaunchService.IsRunning)
                        {
                            _gameLaunchService.StopGame();
                        }
                        Directory.Delete(gameFolder, true); 
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error deleting game folder: {ex.Message}");
                        throw;
                    }
                });
            }

            LocalData = null;
            IsInstalled = false;
            IsUpdated = false;

            StatusMessage = "Game deleted successfully.";
            Progress = 0;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deleting game: {ex.Message}");
            StatusMessage = "Error deleting game.";
        }
    }
    
    public async Task LoadLogoAsync()
    {
        if (!string.IsNullOrEmpty(Game?.LogoUrl))
        {
            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(Game.LogoUrl);

                if (response.IsSuccessStatusCode)
                {
                    await using var stream = await response.Content.ReadAsStreamAsync();
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        LogoBitmap = new Bitmap(stream);
                    });
                }
                else
                {
                    Debug.WriteLine($"Failed to load logo for {Game.Title}: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to load logo for {Game.Title}: {ex.Message}");
            }
        }
    }
    
    private ProductVersionModel? GetGameCurrentVersion()
    {
        var currentPlatform = _installationService.GetCurrentPlatformType();
        return Game?.Versions.FirstOrDefault(v =>
            v.Platform == currentPlatform && 
            v.Version == LocalData?.InstalledVersion);
    }
    
    private List<ProductVersionModel> GetAddonsCurrentVersions()
    {
        var currentPlatform = _installationService.GetCurrentPlatformType();
        return Game?.Addons
            .Where(addon => addon.Platforms.Contains(currentPlatform))
            .Select(addon =>
            {
                var installedAddon = LocalData?.InstalledAddons.FirstOrDefault(ia => ia.Id == addon.Id);
                if (installedAddon == null)
                    return null;

                return addon.Versions
                    .FirstOrDefault(v => v.Platform == currentPlatform && v.Version == installedAddon.InstalledVersion);
            })
            .Where(v => v != null)
            .ToList()!;
    }
}