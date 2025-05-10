namespace Forja.Launcher.Services;

public class GameInstallationService
{
    private readonly ApiService _apiService;

    public GameInstallationService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public Task<bool> CheckForUpdateAsync(LibraryGameModel remoteGame, InstalledGameModel localGame)
    {
        var currentPlatform = GetCurrentPlatformType();

        var latestRemoteVersion = remoteGame.Versions
            .Where(v => v.Platform == currentPlatform)
            .OrderByDescending(v => Version.Parse(v.Version))
            .FirstOrDefault()?.Version;
        
        var gameNeedsUpdate = !string.Equals(latestRemoteVersion, localGame.InstalledVersion, StringComparison.OrdinalIgnoreCase);

        var addonNeedsUpdate = remoteGame.Addons.Any(remoteAddon =>
        {
            var latestAddonVersion = remoteAddon.Versions
                .Where(v => v.Platform == currentPlatform)
                .OrderByDescending(v => Version.Parse(v.Version))
                .FirstOrDefault()?.Version;

            var localAddon = localGame.InstalledAddons.FirstOrDefault(ia => ia.Id == remoteAddon.Id);
            if (localAddon == null)
            {
                return true;
            }

            return !string.Equals(latestAddonVersion, localAddon.InstalledVersion, StringComparison.OrdinalIgnoreCase);
        });
        
        var hasUpdate = gameNeedsUpdate || addonNeedsUpdate;
        return Task.FromResult(hasUpdate);
    }

    public async Task<List<FileToDownloadInfo>> VerifyIntegrityAsync(InstalledGameModel localGame, List<ProductFileModel> expectedFiles)
    {
        var corruptedOrMissing = new List<FileToDownloadInfo>();

        foreach (var expected in expectedFiles)
        {
            var filePath = Path.Combine(localGame.InstallPath, expected.FilePath);

            if (!File.Exists(filePath))
            {
                corruptedOrMissing.Add(new FileToDownloadInfo
                {
                    RelativePath = expected.FilePath,
                    StorageUrl = expected.StorageUrl
                });
                continue;
            }

            try
            {
                await using var stream = File.OpenRead(filePath);
                var hash = await CalculateSha256Async(stream);

                if (!string.Equals(hash, expected.Hash, StringComparison.OrdinalIgnoreCase))
                {
                    corruptedOrMissing.Add(new FileToDownloadInfo
                    {
                        RelativePath = expected.FilePath,
                        StorageUrl = expected.StorageUrl
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error verifying file {filePath}: {ex.Message}");
                corruptedOrMissing.Add(new FileToDownloadInfo
                {
                    RelativePath = expected.FilePath,
                    StorageUrl = expected.StorageUrl
                });
            }
        }

        return corruptedOrMissing;
    }

    public async Task DownloadAndInstallFullAsync(LibraryGameModel game, string installPath, IProgress<double>? progress = null, CancellationToken cancellationToken = default)
    {
        var objectPath = game.Versions.OrderByDescending(v => Version.Parse(v.Version)) 
            .FirstOrDefault()?.StorageUrl;
        if (objectPath is null)
        {
            throw new ArgumentNullException(nameof(objectPath), "Filed to get object path for game");
        }
        Directory.CreateDirectory(installPath);

        var destinationFile = Path.Combine(installPath, $"{game.Title}.zip");

        await _apiService.DownloadFileInChunksAsync(objectPath, destinationFile, progress: progress, cancellationToken: cancellationToken);

        ZipFile.ExtractToDirectory(destinationFile, installPath, overwriteFiles: true);

        File.Delete(destinationFile);
    }

    public async Task DownloadAndReplaceFilesAsync(List<FileToDownloadInfo> filesToDownload, Guid gameId, string baseInstallPath, CancellationToken cancellationToken = default)
    {
        foreach (var fileInfo in filesToDownload)
        {
            var objectPath = fileInfo.StorageUrl;

            var destinationPath = Path.Combine(baseInstallPath, fileInfo.RelativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);

            await _apiService.DownloadFileInChunksAsync(objectPath, destinationPath, cancellationToken: cancellationToken);
        }
    }
    
    public async Task<bool> ApplyPatchIfAvailableAsync(InstalledGameModel localGame, LibraryGameModel remoteGame, IProgress<double>? progress = null, CancellationToken cancellationToken = default)
    {
        var currentPlatform = GetCurrentPlatformType();
        if (remoteGame.Platforms.All(p => p != currentPlatform))
        {
            return false;       
        }
        
        Version.TryParse(localGame.InstalledVersion ?? "0.0.0", out var currentVersion);
        
        var latestVersion = remoteGame.Versions
            .Where(v => v.Platform == currentPlatform)
            .OrderByDescending(v => Version.Parse(v.Version))
            .FirstOrDefault()?.Version;

        if (string.IsNullOrEmpty(latestVersion))
        {
            return false;
        }
        
        var nextPatch = remoteGame.Patches
            .Where(p => Version.Parse(p.FromVersion) == currentVersion)
            .OrderByDescending(p => Version.Parse(p.ToVersion))
            .FirstOrDefault();

        if (nextPatch == null)
        {
            return false;
        }

        Directory.CreateDirectory(localGame.InstallPath);
        var patchFilePath = Path.Combine(localGame.InstallPath, $"{remoteGame.Title}_patch_{nextPatch.ToVersion}.zip");

        await _apiService.DownloadFileInChunksAsync(nextPatch.PatchUrl, patchFilePath, progress: progress, cancellationToken: cancellationToken);
        ZipFile.ExtractToDirectory(patchFilePath, localGame.InstallPath, overwriteFiles: true);
        File.Delete(patchFilePath);

        localGame.InstalledVersion = nextPatch.ToVersion;
        return true;
    }

    public async Task InstallAddonAsync(InstalledGameModel game, LibraryAddonModel addon, IProgress<double>? progress = null, CancellationToken cancellationToken = default)
    {
        var latestVersion = addon.Versions
            .OrderByDescending(v => Version.Parse(v.Version))
            .FirstOrDefault();

        if (latestVersion == null)
        {
            throw new InvalidOperationException("No version available for addon.");
        }

        var addonPath = Path.Combine(game.InstallPath, "addons", addon.Name);
        Directory.CreateDirectory(addonPath);

        var archivePath = Path.Combine(addonPath, $"{addon.Name}_{latestVersion.Version}.zip");
        await _apiService.DownloadFileInChunksAsync(latestVersion.StorageUrl, archivePath, 1048576, progress, cancellationToken);
        ZipFile.ExtractToDirectory(archivePath, addonPath, overwriteFiles: true);
        File.Delete(archivePath);

        var installedAddon = game.InstalledAddons.FirstOrDefault(a => a.Id == addon.Id);
        if (installedAddon != null)
        {
            installedAddon.InstalledVersion = latestVersion.Version;
            installedAddon.Files = latestVersion.Files.Select(ModelMapper.MapToIbInstalledFileModel).ToList();
        }
    }

    private async Task<string> CalculateSha256Async(Stream stream)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = await sha256.ComputeHashAsync(stream);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
    }
    
    public PlatformType GetCurrentPlatformType()
    {
        return Environment.OSVersion.Platform switch
        {
            PlatformID.Win32NT => PlatformType.Windows,
            PlatformID.Unix => PlatformType.Linux,
            PlatformID.MacOSX => PlatformType.Mac,
            _ => PlatformType.Windows
        };
    }
}