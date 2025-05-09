namespace Forja.Launcher.Maping;

public static class ModelMapper
{
    public static InstalledGameModel MapToInstalledGame(LibraryGameModel dto, string installPath)
    {
        return new InstalledGameModel
        {
            Id = dto.Id,
            Title = dto.Title,
            LogoUrl = dto.LogoUrl,
            Platform = DetectPlatform(dto.Platforms), 
            InstallPath = installPath,
            InstalledVersion = "", // set after install
            InstalledAddons = dto.Addons.Select(MapToInstalledAddon).ToList(),
            AvailablePatches = dto.Patches,
            Files = [] // fill when installed
        };
    }

    public static InstalledAddonModel MapToInstalledAddon(LibraryAddonModel dto)
    {
        return new InstalledAddonModel
        {
            Id = dto.Id,
            Name = dto.Name,
            InstalledVersion = "", // set after install
            Files = [] // fill when installed
        };
    }

    public static InstalledFileModel MapToIbInstalledFileModel(ProductFileModel dto)
    {
        return new InstalledFileModel
        {
            Id = dto.Id,
            FileName = dto.FileName,
            FilePath = dto.FilePath,
            FileSize = dto.FileSize,
            Hash = dto.Hash,
            IsArchive = dto.IsArchive,
            Status = FileStatus.Valid
        };
    }
    
    private static PlatformType DetectPlatform(List<PlatformType> platforms)
    {
        var currentPlatform = Environment.OSVersion.Platform switch
        {
            PlatformID.Win32NT => PlatformType.Windows,
            PlatformID.Unix => PlatformType.Linux,
            PlatformID.MacOSX => PlatformType.Mac,
            _ => PlatformType.Windows
        };
    
        return platforms.Contains(currentPlatform) 
            ? currentPlatform 
            : platforms.FirstOrDefault();
    }
}