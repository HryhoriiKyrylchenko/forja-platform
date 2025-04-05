namespace Forja.API.Controllers.Common;

[ApiController]
[Route("api/[controller]")]
public class FilterDataController : ControllerBase
{
    private readonly IFilterDataService _filterDataService;
    private readonly IAuditLogService _auditLogService;

    public FilterDataController(IFilterDataService filterDataService,
        IAuditLogService auditLogService)
    {
        _filterDataService = filterDataService;
        _auditLogService = auditLogService;
    }

    /// <summary>
    /// Gets the filter data for products.
    /// </summary>
    /// <remarks>
    /// This endpoint retrieves genres, mechanics, tags, and mature content details to be used in product filtering.
    /// </remarks>
    /// <returns>A collection of filter data.</returns>
    [HttpGet("product-filters")]
    public async Task<ActionResult<ProductsFilterDataDto>> GetProductFiltersDataAsync()
    {
        try
        {
            var filterData = await _filterDataService.GetGameFilterDataAsync();
            return Ok(filterData);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.View,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to get game filter data." }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return StatusCode(500, "An error occurred while fetching filter data.");
        }
    }
}