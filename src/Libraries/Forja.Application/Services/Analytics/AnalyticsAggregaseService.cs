namespace Forja.Application.Services.Analytics;

public class AnalyticsAggregaseService : IAnalyticsAggregaseService
{
    private readonly IAnalyticsAggregateRepository _analyticsAggregateRepository;
    private readonly IAnalyticsEventRepository _analyticsEventRepository;
    private readonly IAnalyticsSessionRepository _analyticsSessionRepository;

    public AnalyticsAggregaseService(IAnalyticsAggregateRepository analyticsAggregateRepository,
        IAnalyticsEventRepository analyticsEventRepository,
        IAnalyticsSessionRepository analyticsSessionRepository)
    {
        _analyticsAggregateRepository = analyticsAggregateRepository;
        _analyticsEventRepository = analyticsEventRepository;
        _analyticsSessionRepository = analyticsSessionRepository;
    }
    
    public async Task<List<AnalyticsAggregateDto>> GetAnalyticsAggregateOfSessionsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        if (!startDate.HasValue && !endDate.HasValue && startDate > endDate)
        {
            throw new ArgumentException("Invalid date range. Ensure both start and end dates are provided and startDate <= endDate.");
        }
        
        const string metricNameSessions = "Sessions";
        
        var startDateValue = startDate ?? DateTime.UtcNow;
        var endDateValue = endDate ?? DateTime.UtcNow;

        var existingAggregates = await _analyticsAggregateRepository.GetAllAsync(startDate, endDate);

        if (existingAggregates == null)
        {
            throw new Exception("Failed to fetch existing aggregates.");
        }
        
        var dateToAggregateMap = existingAggregates
            .Where(a => a.MetricName == metricNameSessions)
            .ToDictionary(a => a.Date.Date, a => a);
        
        var results = new List<AnalyticsAggregateDto>();
        
        for (var date = startDateValue.Date; date <= endDateValue.Date; date = date.AddDays(1))
        {
            if (dateToAggregateMap.TryGetValue(date, out var existingAggregate))
            {
                results.Add(AnalyticsEntityToDtoMapper.MapToAnalyticsAggregateDto(existingAggregate));
            }
            else
            {
                var sessionsCount = await _analyticsSessionRepository.GetSessionCountAsync(date, date);
                var newAggregate = new AnalyticsAggregate
                {
                    Id = Guid.NewGuid(),
                    Date = date,
                    MetricName = metricNameSessions,
                    Value = sessionsCount
                };

                try
                {
                    var addedAggregate = await _analyticsAggregateRepository.AddAsync(newAggregate);
                    if (addedAggregate == null)
                    {
                        throw new Exception($"Failed to add a new aggregate for date {date:yyyy-MM-dd}.");
                    }

                    results.Add(AnalyticsEntityToDtoMapper.MapToAnalyticsAggregateDto(addedAggregate));
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred while adding a new aggregate for date {date:yyyy-MM-dd}.", ex);
                }
            }
        }

        return results;
    }

    public async Task<List<AnalyticsAggregateDto>> GetAnalyticsAggregateOfEventsAsync(AnalyticEventType eventType, DateTime? startDate = null, DateTime? endDate = null)
    {
        if (startDate.HasValue && endDate.HasValue && startDate > endDate)
        {
            throw new ArgumentException("Invalid date range. Ensure both start and end dates are provided and startDate <= endDate.");
        }

        var metricName = eventType.ToString();

        var startDateValue = startDate ?? DateTime.UtcNow.Date;
        var endDateValue = endDate ?? DateTime.UtcNow.Date; 

        var existingAggregates = await _analyticsAggregateRepository.GetAllAsync(startDate, endDate);

        if (existingAggregates == null)
        {
            throw new Exception("Failed to retrieve existing aggregates from the repository.");
        }

        var dateToAggregateMap = existingAggregates
            .Where(a => a.MetricName == metricName)
            .ToDictionary(a => a.Date.Date, a => a);

        var results = new List<AnalyticsAggregateDto>();

        for (var date = startDateValue; date <= endDateValue; date = date.AddDays(1))
        {
            if (dateToAggregateMap.TryGetValue(date, out var existingAggregate))
            {
                results.Add(AnalyticsEntityToDtoMapper.MapToAnalyticsAggregateDto(existingAggregate));
            }
            else
            {
                var eventsForDate = await _analyticsEventRepository.GetAllAsync(
                    startDate: date,
                    endDate: date.AddDays(1),
                    eventType: eventType
                );

                var eventCount = eventsForDate?.Count() ?? 0;

                var newAggregate = new AnalyticsAggregate
                {
                    Id = Guid.NewGuid(),
                    Date = date,
                    MetricName = metricName,
                    Value = eventCount
                };

                try
                {
                    var addedAggregate = await _analyticsAggregateRepository.AddAsync(newAggregate);

                    if (addedAggregate == null)
                    {
                        throw new Exception($"Failed to add a new aggregate for date {date:yyyy-MM-dd}, eventType {metricName}.");
                    }

                    results.Add(AnalyticsEntityToDtoMapper.MapToAnalyticsAggregateDto(addedAggregate));
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred while adding a new aggregate for date {date:yyyy-MM-dd}.", ex);
                }
            }
        }

        return results;
    }
}