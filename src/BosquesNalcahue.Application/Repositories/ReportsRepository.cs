using BosquesNalcahue.Application.Entities;
using BosquesNalcahue.Application.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace BosquesNalcahue.Application.Repositories;

public class ReportsRepository : IReportsRepository
{
    private readonly IMongoCollection<BaseReport> _reportsCollection;

    public ReportsRepository(IMongoDbOptions mongoDb)
    {
        MongoClient client = new(mongoDb.Server);
        IMongoDatabase database = client.GetDatabase(mongoDb.Database);
        _reportsCollection = database.GetCollection<BaseReport>(mongoDb.Collection);
    }

    public async Task CreateAsync(BaseReport report, CancellationToken token = default)
    {
        try
        {
            await _reportsCollection.InsertOneAsync(report, cancellationToken: token);
        }
        catch (Exception ex)
        {
            throw new Exception("Error creating report", ex);
        }
    }

    public async Task<bool> DeleteByIdAsync(ObjectId id, CancellationToken token = default)
    {
        var filter = Builders<BaseReport>.Filter.Eq(report => report.Id, id);

        // Find the document to provide a better response if it doesn't exist
        var documentExists = await _reportsCollection.Find(filter).AnyAsync(cancellationToken: token);

        if (!documentExists)
            return false;
        
        var deleteResult = await _reportsCollection.DeleteOneAsync(filter, cancellationToken: token);

        return deleteResult.DeletedCount > 0;
    }

    public async Task<IEnumerable<BaseReport>> GetAllAsync(
        FilteringOptions options, CancellationToken token = default)
    {
        var filter = GenerateFilter(options);

        var documents = await _reportsCollection.FindAsync(filter, cancellationToken: token).Result
                                                .ToListAsync(cancellationToken: token);
        
        return documents;
    }

    public async Task<BaseReport> GetByIdAsync(ObjectId id, CancellationToken token = default)
    {
        var filter = Builders<BaseReport>.Filter.Eq(report => report.Id, id);
        var report = await _reportsCollection.Find(filter).FirstOrDefaultAsync(cancellationToken: token);
        return report;
    }

    public FilterDefinition<BaseReport> GenerateFilter(FilteringOptions options)
    {
        var filterBuilder = Builders<BaseReport>.Filter;
        var filter = filterBuilder.Empty;

        if (!string.IsNullOrWhiteSpace(options.OperatorName))
        {
            var regex = new Regex(options.OperatorName, RegexOptions.IgnoreCase);

            var operatorNameFilter = filterBuilder.Regex(report => report.OperatorName,
                               new BsonRegularExpression(regex));
            
            filter &= operatorNameFilter;
        }
            

        if (options.StartDate.HasValue)
            filter &= filterBuilder.Gte(report => report.Date, options.StartDate);

        if (options.EndDate.HasValue)
            filter &= filterBuilder.Lte(report => report.Date, options.EndDate);

        if (!string.IsNullOrWhiteSpace(options.ProductType))
            filter &= filterBuilder.Eq(report => report.ProductType, options.ProductType);

        if (options.Species is not null && options.Species.Any())
            filter &= filterBuilder.All(report => report.Species, options.Species);

        return filter;
    }

    public async Task<bool> ReplaceAsync(BaseReport report, CancellationToken token = default)
    {
        var filter = Builders<BaseReport>.Filter.Eq(r => r.Id, report.Id);
        
        var result = await _reportsCollection.ReplaceOneAsync(filter, report, cancellationToken: token);
        
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }
}
