namespace Forja.Application.Services.Analytics;

public class AnalyticsAggregateService : IAnalyticsAggregateService
{
    private readonly IAnalyticsAggregateRepository _analyticsAggregateRepository;
    private readonly IAnalyticsEventRepository _analyticsEventRepository;
    private readonly IAnalyticsSessionRepository _analyticsSessionRepository;

    public AnalyticsAggregateService(IAnalyticsAggregateRepository analyticsAggregateRepository,
        IAnalyticsEventRepository analyticsEventRepository,
        IAnalyticsSessionRepository analyticsSessionRepository)
    {
        _analyticsAggregateRepository = analyticsAggregateRepository;
        _analyticsEventRepository = analyticsEventRepository;
        _analyticsSessionRepository = analyticsSessionRepository;
    }
    
    /// <inheritdoc />
    public async Task<List<AnalyticsAggregateDto>> GetAnalyticsAggregateOfSessionsAsync(DateTime startDate, DateTime? endDate = null)
    {
        if (startDate > DateTime.UtcNow)
        {
            throw new ArgumentException("Invalid start date. Ensure start date is not in the future.");
        }

        if (endDate.HasValue && startDate.Date > endDate.Value.Date)
        {
            throw new ArgumentException("Invalid date range. Ensure both start and end dates are provided and startDate <= endDate.");
        }

        if ((endDate.HasValue && endDate.Value > DateTime.UtcNow) || endDate == null)
        {
            endDate = DateTime.UtcNow;
        }
        
        const string metricNameSessions = "Sessions";
        
        var existingAggregates = await _analyticsAggregateRepository.GetAllAsync(startDate, endDate);

        if (existingAggregates == null)
        {
            throw new Exception("Failed to fetch existing aggregates.");
        }
        
        var dateToAggregateMap = existingAggregates
            .Where(a => a.MetricName == metricNameSessions)
            .ToDictionary(a => a.Date.Date, a => a);
        
        var results = new List<AnalyticsAggregateDto>();
        
        for (var date = startDate.Date; date <= endDate.Value.Date; date = date.AddDays(1))
        {
            if (dateToAggregateMap.TryGetValue(date, out var existingAggregate))
            {
                if (date == DateTime.UtcNow.Date)
                {
                    var newSessionsCount = await _analyticsSessionRepository.GetSessionCountAsync(date);
                    if (newSessionsCount != existingAggregate.Value)
                    {
                        existingAggregate.Value = newSessionsCount;
                        existingAggregate = await _analyticsAggregateRepository.UpdateAsync(existingAggregate);
                        if (existingAggregate == null)
                        {
                            throw new Exception($"Failed to update aggregate for date {date:yyyy-MM-dd}.");
                        }
                    }
                }
                results.Add(AnalyticsEntityToDtoMapper.MapToAnalyticsAggregateDto(existingAggregate));
            }
            else
            {
                try
                {
                    var sessionsCount = await _analyticsSessionRepository.GetSessionCountAsync(date);
                    var newAggregate = new AnalyticsAggregate
                    {
                        Id = Guid.NewGuid(),
                        Date = date,
                        MetricName = metricNameSessions,
                        Value = sessionsCount
                    };
                    
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

    /// <inheritdoc />
    public async Task<List<AnalyticsAggregateDto>> GetAnalyticsAggregateOfEventsAsync(AnalyticEventType eventType, DateTime startDate, DateTime? endDate = null)
    {
        if (startDate > DateTime.UtcNow)
        {
            throw new ArgumentException("Invalid start date. Ensure start date is not in the future.");
        }

        if (endDate.HasValue && startDate.Date > endDate.Value.Date)
        {
            throw new ArgumentException("Invalid date range. Ensure both start and end dates are provided and startDate <= endDate.");
        }

        if ((endDate.HasValue && endDate.Value > DateTime.UtcNow) || endDate == null)
        {
            endDate = DateTime.UtcNow;
        }

        var metricName = eventType.ToString();

        var existingAggregates = await _analyticsAggregateRepository.GetAllAsync(startDate, endDate);

        if (existingAggregates == null)
        {
            throw new Exception("Failed to retrieve existing aggregates from the repository.");
        }

        var dateToAggregateMap = existingAggregates
            .Where(a => a.MetricName == metricName)
            .ToDictionary(a => a.Date.Date, a => a);

        var results = new List<AnalyticsAggregateDto>();

        for (var date = startDate.Date; date <= endDate.Value.Date; date = date.AddDays(1))
        {
            if (dateToAggregateMap.TryGetValue(date, out var existingAggregate))
            {
                if (date == DateTime.UtcNow.Date)
                {
                    var newEventCount = await _analyticsEventRepository.GetEventsCountAsync(date, eventType);
                    if (newEventCount != existingAggregate.Value)
                    {
                        existingAggregate.Value = newEventCount;
                        existingAggregate = await _analyticsAggregateRepository.UpdateAsync(existingAggregate);
                        if (existingAggregate == null)
                        {
                            throw new Exception($"Failed to update aggregate for date {date:yyyy-MM-dd}.");
                        }
                    }
                }
                results.Add(AnalyticsEntityToDtoMapper.MapToAnalyticsAggregateDto(existingAggregate));
            }
            else
            {
                try
                {
                    var eventCount = await _analyticsEventRepository.GetEventsCountAsync(date, eventType);
                    var newAggregate = new AnalyticsAggregate
                    {
                        Id = Guid.NewGuid(),
                        Date = date,
                        MetricName = metricName,
                        Value = eventCount
                    };
                    
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