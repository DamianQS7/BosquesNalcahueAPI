
using BosquesNalcahue.Application.Entities;
using BosquesNalcahue.Application.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace BosquesNalcahue.Application.Repositories;

public class AnalyticsRepository : IAnalyticsRepository
{
    private readonly IMongoCollection<BaseReport> _reportsCollection;

    public AnalyticsRepository(IMongoDbOptions options)
    {
        MongoClient client = new(options.Server);
        IMongoDatabase database = client.GetDatabase(options.Database);
        _reportsCollection = database.GetCollection<BaseReport>(options.Collection);
    }

    public async Task<IEnumerable<ReportsCountDocument>> GetMonthlyCountBreakdownAsync(CancellationToken token = default)
    {
        var results = _reportsCollection.AsQueryable()
           .Where(doc => doc.Date.Year == DateTime.Now.Year) // To display only the current year monthly breakdown
           .GroupBy(group => new { group.ProductType, group.Date.Month, group.Date.Year }) // Group by ProductType, month, and year
           .Select(group => new ReportsCountDocument { ProductType = group.Key.ProductType, Month = group.Key.Month, Count = group.Count() })
           .ToListAsync(cancellationToken: token);

        return await results;
    }

    public async Task<IEnumerable<ReportsCountDocument>> GetReportCountByMonthAsync(int month, CancellationToken token = default)
    {
        var results = _reportsCollection.AsQueryable()
            .Where(doc => doc.Date.Month == month)
            .GroupBy(doc => doc.ProductType)
            .Select(group => new ReportsCountDocument { ProductType = group.Key, Count = group.Count() })
            .ToListAsync(cancellationToken: token);

        return await results;
    }

    public async Task<IEnumerable<ReportsCountDocument>> GetReportCountByPeriodAsync(GetAnalyticsByPeriodOptions options, CancellationToken token = default)
    {
        var query = _reportsCollection.AsQueryable();

        if (options.StartDate is not null && options.EndDate is not null)
            query = query.Where(doc => doc.Date >= options.StartDate && doc.Date < options.EndDate.Value.AddDays(1));

        var results = query
            .GroupBy(doc => doc.ProductType)
            .Select(group => new ReportsCountDocument { ProductType = group.Key, Count = group.Count() })
            .ToListAsync(cancellationToken: token);

        return await results;
    }
}
