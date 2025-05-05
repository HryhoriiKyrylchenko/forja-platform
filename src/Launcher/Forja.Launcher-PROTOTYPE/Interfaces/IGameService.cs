namespace Forja.Launcher.Interfaces;

public interface IGameService
{
    Task InstallAsync(GameViewModel gameVm);
    Task UpdateAsync(GameViewModel gameVm);
    Task VerifyFilesAsync(GameViewModel gameVm);
    Task DeleteAsync(GameViewModel gameVm);
}