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
            var filePath = Path.Combine(localGame.InstallPath, expected.FilePath, expected.FileName);

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
        var currentPlatform = GetCurrentPlatformType();
        var objectPath = game.Versions
            .Where(v => v.Platform == currentPlatform)
            .OrderByDescending(v => Version.Parse(v.Version)) 
            .FirstOrDefault()?.StorageUrl;
        if (objectPath is null)
        {
            throw new ArgumentNullException(nameof(objectPath), "Filed to get object path for game");
        }
        Directory.CreateDirectory(installPath);

        var destinationFile = Path.Combine(installPath, $"{game.Title}.zip");
        var tempExtractPath = Path.Combine(installPath, "__temp_unpack");

        await _apiService.DownloadFileInChunksAsync(objectPath, destinationFile, progress: progress, cancellationToken: cancellationToken);

        if (Directory.Exists(tempExtractPath))
            Directory.Delete(tempExtractPath, recursive: true);

        Directory.CreateDirectory(tempExtractPath);
        
        ZipFile.ExtractToDirectory(destinationFile, tempExtractPath, overwriteFiles: true);
        
        var extractedEntries = Directory.GetFileSystemEntries(tempExtractPath);
        string actualRoot = tempExtractPath;

        if (extractedEntries.Length == 1 && Directory.Exists(extractedEntries[0]))
        {
            actualRoot = extractedEntries[0]; 
        }

        foreach (var file in Directory.GetFiles(actualRoot, "*", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(actualRoot, file);
            var targetPath = Path.Combine(installPath, relativePath);
            var targetDir = Path.GetDirectoryName(targetPath)!;

            Directory.CreateDirectory(targetDir);
            File.Move(file, targetPath, overwrite: true);
        }
        
        File.Delete(destinationFile);
        Directory.Delete(tempExtractPath, recursive: true);
    }

    public async Task DownloadAndReplaceFilesAsync(List<FileToDownloadInfo> filesToDownload,string baseInstallPath, CancellationToken cancellationToken = default)
    {
        foreach (var fileInfo in filesToDownload)
        {
            var objectPath = fileInfo.StorageUrl;

            var installationPath = Path.Combine(baseInstallPath, fileInfo.RelativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(installationPath)!);
            var fileName = Path.GetFileName(fileInfo.StorageUrl);
            var destinationPath = Path.Combine(installationPath, fileName);

            await _apiService.DownloadFileInChunksAsync(objectPath, destinationPath, cancellationToken: cancellationToken);
        }
    }
    
    public async Task<bool> ApplyPatchIfAvailableAsync(InstalledGameModel localGame, LibraryGameModel remoteGame, IProgress<double>? progress = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(localGame.InstalledVersion) 
            || localGame.InstalledVersion == "0.0.0"
            || string.IsNullOrWhiteSpace(localGame.InstallPath))
        {
            return false;
        }
        
        var currentPlatform = GetCurrentPlatformType();
        if (remoteGame.Platforms.All(p => p != currentPlatform))
        {
            return false;       
        }
        
        Version.TryParse(localGame.InstalledVersion, out var currentVersion);
        
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

        var patchFilePath = Path.Combine(localGame.InstallPath, $"{remoteGame.Title}_patch_{nextPatch.ToVersion}.zip");

        await _apiService.DownloadFileInChunksAsync(nextPatch.PatchUrl, patchFilePath, progress: progress, cancellationToken: cancellationToken);
        
        var tempExtractPath = Path.Combine(localGame.InstallPath, $"_temp_patch_{Guid.NewGuid()}");
        Directory.CreateDirectory(tempExtractPath);
        ZipFile.ExtractToDirectory(patchFilePath, tempExtractPath, overwriteFiles: true);
        
        var extractedEntries = Directory.GetFileSystemEntries(tempExtractPath);
        string actualRoot = tempExtractPath;

        if (extractedEntries.Length == 1 && Directory.Exists(extractedEntries[0]))
        {
            actualRoot = extractedEntries[0];
        }

        foreach (var file in Directory.GetFiles(actualRoot, "*", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(actualRoot, file);
            var targetPath = Path.Combine(localGame.InstallPath, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(targetPath)!);
            File.Move(file, targetPath, overwrite: true);
        }
        
        File.Delete(patchFilePath);
        Directory.Delete(tempExtractPath, recursive: true);

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

        var archivePath = Path.Combine(game.InstallPath, $"{addon.Name}_{latestVersion.Version}.zip");
        await _apiService.DownloadFileInChunksAsync(latestVersion.StorageUrl, archivePath, 1048576, progress, cancellationToken);
        
        var tempExtractPath = Path.Combine(game.InstallPath, $"_temp_addon_{Guid.NewGuid()}");
        Directory.CreateDirectory(tempExtractPath);
        ZipFile.ExtractToDirectory(archivePath, tempExtractPath, overwriteFiles: true);

        var extractedEntries = Directory.GetFileSystemEntries(tempExtractPath);
        var actualRoot = tempExtractPath;
        if (extractedEntries.Length == 1 && Directory.Exists(extractedEntries[0]))
        {
            actualRoot = extractedEntries[0];
        }

        foreach (var file in Directory.GetFiles(actualRoot, "*", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(actualRoot, file);
            var targetPath = Path.Combine(game.InstallPath, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(targetPath)!);
            File.Move(file, targetPath, overwrite: true);
        }
        
        File.Delete(archivePath);
        Directory.Delete(tempExtractPath, recursive: true);

        var installedAddon = game.InstalledAddons.FirstOrDefault(a => a.Id == addon.Id);
        if (installedAddon != null)
        {
            installedAddon.InstalledVersion = latestVersion.Version;
            installedAddon.Files = latestVersion.Files.Select(ModelMapper.MapToIbInstalledFileModel).ToList();
        }
        else
        {
            var newAddon = ModelMapper.MapToInstalledAddon(addon);
            newAddon.InstalledVersion = latestVersion.Version;
            newAddon.Files = latestVersion.Files.Select(ModelMapper.MapToIbInstalledFileModel).ToList();
            game.InstalledAddons.Add(newAddon);
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