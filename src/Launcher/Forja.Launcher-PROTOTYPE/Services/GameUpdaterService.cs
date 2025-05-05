// using System.IO.Compression;
// using System.Security.Cryptography;
//
// namespace Forja.Launcher.Services;
//
// public class GameUpdaterService
// {
//     private readonly ApiService _apiService;
//
//     public GameUpdaterService(ApiService apiService)
//     {
//         _apiService = apiService;
//         
//     }
//
//     public async Task UpdateGameAsync(LibraryGameInfo game)
//     {
//         try
//         {
//
//
//             // Step 1: Integrity check
//             var isOk = await VerifyInstallationAsync(localVersion, gameDir);
//             if (isOk)
//             {
//                 Debug.WriteLine("Game is already up to date and valid.");
//                 return;
//             }
//
//             // Step 2: Try patching
//             var currentVersion = await DetectInstalledVersionAsync(gameDir, game.Versions);
//             var patch = game.Patches
//                 .FirstOrDefault(p => p.FromVersion == currentVersion?.Version && p.ToVersion == localVersion.Version);
//
//             if (patch != null)
//             {
//                 Debug.WriteLine($"Patch available from {patch.FromVersion} to {patch.ToVersion}. Applying patch...");
//                 await DownloadAndApplyPatchAsync(patch, gameDir, game);
//             }
//             else
//             {
//                 Debug.WriteLine("No patch available or unknown version. Downloading full version...");
//                 await DownloadAndInstallFullVersionAsync(localVersion, gameDir, game);
//             }
//
//             Debug.WriteLine("Update complete.");
//         }
//         catch (Exception ex)
//         {
//             Debug.WriteLine($"Error updating game: {ex}");
//         }
//     }
//
//     private async Task<bool> VerifyInstallationAsync(GameVersionInfo version, string gameInstallDir)
//     {
//         foreach (var file in version.Files)
//         {
//             var fullPath = Path.Combine(gameInstallDir, file.FilePath);
//             if (!File.Exists(fullPath))
//             {
//                 Debug.WriteLine($"Missing file: {file.FilePath}");
//                 return false;
//             }
//
//             if (!await HashMatchesAsync(fullPath, file.Hash))
//             {
//                 Debug.WriteLine($"Corrupted file: {file.FilePath}");
//                 return false;
//             }
//         }
//         return true;
//     }
//
//     private async Task<GameVersionInfo?> DetectInstalledVersionAsync(string gameInstallDir, List<GameVersionInfo> versions)
//     {
//         foreach (var version in versions.OrderByDescending(v => v.Version))
//         {
//             var isValid = await VerifyInstallationAsync(version, gameInstallDir);
//             if (isValid)
//                 return version;
//         }
//         return null;
//     }
//
//     private async Task DownloadAndInstallFullVersionAsync(GameVersionInfo version, string gameInstallDir, LibraryGameInfo game)
//     {
//         var tempZipPath = Path.Combine(Path.GetTempPath(), $"{game.Id}_v{version.Version}.zip");
//
//         var progressReporter = new Progress<double>(p =>
//         {
//             Debug.WriteLine($"Full version download progress: {p:P0}");
//         });
//
//         await _apiService.DownloadFileInChunksAsync(version.StorageUrl, tempZipPath, progress: progressReporter);
//
//         if (!await HashMatchesAsync(tempZipPath, version.Hash))
//             throw new InvalidOperationException("Downloaded full version file is corrupted!");
//
//         if (Directory.Exists(gameInstallDir))
//             Directory.Delete(gameInstallDir, true);
//
//         Directory.CreateDirectory(gameInstallDir);
//
//         ZipFile.ExtractToDirectory(tempZipPath, gameInstallDir);
//
//         // After extraction, verify integrity one more time
//         var isOk = await VerifyInstallationAsync(version, gameInstallDir);
//         if (!isOk)
//             throw new InvalidOperationException("Installation integrity check failed after extraction.");
//     }
//
//     private async Task DownloadAndApplyPatchAsync(GamePatchInfo patch, string gameInstallDir, LibraryGameInfo game)
//     {
//         var tempZipPath = Path.Combine(Path.GetTempPath(), $"{game.Id}_patch_{patch.Id}.zip");
//
//         var progressReporter = new Progress<double>(p =>
//         {
//             Debug.WriteLine($"Patch download progress: {p:P0}");
//         });
//
//         await _apiService.DownloadFileInChunksAsync(patch.PatchUrl, tempZipPath, progress: progressReporter);
//
//         if (!await HashMatchesAsync(tempZipPath, patch.Hash))
//             throw new InvalidOperationException("Downloaded patch file is corrupted!");
//
//         ZipFile.ExtractToDirectory(tempZipPath, gameInstallDir, overwriteFiles: true);
//
//         // You may want to detect the new version after patching
//         var version = game.Versions.FirstOrDefault(v => v.Version == patch.ToVersion);
//         if (version != null)
//         {
//             var isOk = await VerifyInstallationAsync(version, gameInstallDir);
//             if (!isOk)
//                 throw new InvalidOperationException("Integrity check failed after patching.");
//         }
//     }
//
//     private static async Task<bool> HashMatchesAsync(string filePath, string expectedHash)
//     {
//         using var sha256 = SHA256.Create();
//         await using var stream = File.OpenRead(filePath);
//         var hashBytes = await sha256.ComputeHashAsync(stream);
//         var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
//         return hash == expectedHash.ToLowerInvariant();
//     }
// }