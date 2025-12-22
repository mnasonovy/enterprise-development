using Library.Application.Contracts.Analytics;
using Library.Domain.RepositoryInterfaces;
using Microsoft.Extensions.Logging;

namespace Library.Application.Services;

public class AnalyticsService(
    IAnalyticsRepository analyticsRepository,
    ILogger<AnalyticsService> logger) : IAnalyticsService
{
    public async Task<IReadOnlyList<string>> GetIssuedBooksOrderedByTitleAsync()
    {
        logger.LogInformation("{Method} called", nameof(GetIssuedBooksOrderedByTitleAsync));
        return await analyticsRepository.GetIssuedBooksOrderedByTitleAsync();
    }

    public async Task<IReadOnlyList<TopReaderDto>> GetTopReadersAsync()
    {
        logger.LogInformation("{Method} called", nameof(GetTopReadersAsync));
        return await analyticsRepository.GetTopReadersAsync();
    }

    public async Task<IReadOnlyList<ReaderDaysCountDto>> GetReadersByDaysCountAsync()
    {
        logger.LogInformation("{Method} called", nameof(GetReadersByDaysCountAsync));
        return await analyticsRepository.GetReadersByDaysCountAsync();
    }

    public async Task<IReadOnlyList<TopPublisherDto>> GetTopPublishersLastYearAsync()
    {
        logger.LogInformation("{Method} called", nameof(GetTopPublishersLastYearAsync));
        return await analyticsRepository.GetTopPublishersLastYearAsync();
    }

    public async Task<IReadOnlyList<TopBookDto>> GetTopPopularBooksLastYearAsync()
    {
        logger.LogInformation("{Method} called", nameof(GetTopPopularBooksLastYearAsync));
        return await analyticsRepository.GetTopPopularBooksLastYearAsync();
    }
}
