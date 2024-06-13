using BosquesNalcahue.Application.Models;

namespace BosquesNalcahue.Application.Repositories;

public interface IAnalyticsRepository
{
    Task<IEnumerable<ReportsCountDocument>> GetReportCountByMonthAsync(int month, CancellationToken token = default);
    Task<IEnumerable<ReportsCountDocument>> GetReportCountByPeriodAsync(GetAnalyticsByPeriodOptions options, CancellationToken token = default);
}
