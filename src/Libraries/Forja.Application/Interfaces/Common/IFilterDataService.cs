namespace Forja.Application.Interfaces.Common;

public interface IFilterDataService
{
    Task<ProductsFilterDataDto> GetGameFilterDataAsync();
}