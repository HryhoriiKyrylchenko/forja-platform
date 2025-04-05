namespace Forja.Application.Interfaces.Common;

public interface IHomeService
{
    Task<HomepageDto> GetHomepageDataAsync();
}