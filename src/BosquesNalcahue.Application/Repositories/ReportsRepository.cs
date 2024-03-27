using BosquesNalcahue.Application.Entities;
using BosquesNalcahue.Application.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Globalization;
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
            throw new Exception("There was an unexpected error creating the report", ex);
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

    //public async Task<IEnumerable<BaseReport>> GetAllAsync(
    //    FilteringOptions options, CancellationToken token = default)
    //{
    //    var filter = GenerateFilter(options);

    //    var sort = SortDocuments(options);

    //    var documents = await _reportsCollection.Find(filter)
    //                                            .Sort(sort)
    //                                            .ToListAsync(cancellationToken: token);

    //    return documents;
    //}

    public async Task<IEnumerable<BaseReport>> GetAllAsync(
        GetAllReportsOptions options, CancellationToken token = default)
    {
        var collection = _reportsCollection.AsQueryable();

        // Filtering
        if (!string.IsNullOrWhiteSpace(options.OperatorName))
        {
            var regex = new Regex(options.OperatorName, RegexOptions.IgnoreCase);

            collection = collection.Where(report => regex.IsMatch(report.OperatorName!));
        }

        if (options.StartDate.HasValue)
            collection = collection.Where(report => report.Date >= options.StartDate);

        if (options.EndDate.HasValue)
            collection = collection.Where(report => report.Date <= options.EndDate);

        if (!string.IsNullOrWhiteSpace(options.ProductType))
            collection = collection.Where(report => report.ProductType == options.ProductType);

        if (options.Species is not null && options.Species.Any())
        {
            foreach (var species in options.Species)
            {
                collection = collection.Where(report => report.Species!.Contains(species));
            }
        }

        if (!string.IsNullOrEmpty(options.ProductName))
            collection = collection.Where(report => report.ProductName == options.ProductName);

        if (!string.IsNullOrEmpty(options.SortBy))
        {
            collection = options.SortBy.ToLower() switch
            {
                "operatorname" => options.SortOrder == SortOrder.Ascending
                                        ? collection.OrderBy(d => d.OperatorName)
                                        : collection.OrderByDescending(d => d.OperatorName),
                "clientName" => options.SortOrder == SortOrder.Ascending
                                        ? collection.OrderBy(d => d.ClientName)
                                        : collection.OrderByDescending(d => d.ClientName),
                _ => options.SortOrder == SortOrder.Ascending
                                        ? collection.OrderBy(d => d.Date)
                                        : collection.OrderByDescending(d => d.Date),
            };
        }
        else
        {
            collection = collection.OrderByDescending(d => d.Date);
        }

        return await collection.ToListAsync(cancellationToken: token);
    }

    public async Task<BaseReport> GetByIdAsync(ObjectId id, CancellationToken token = default)
    {
        var filter = Builders<BaseReport>.Filter.Eq(report => report.Id, id);
        var report = await _reportsCollection.Find(filter).FirstOrDefaultAsync(cancellationToken: token);
        return report;
    }

    public void FilterAndSort(GetAllReportsOptions options, IMongoQueryable<BaseReport> collection)
    {
        
    }

    public static FilterDefinition<BaseReport> GenerateFilter(GetAllReportsOptions options)
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

    public static SortDefinition<BaseReport> SortDocuments(GetAllReportsOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.SortBy))
        {
            if (options.SortOrder == SortOrder.Ascending)
                return Builders<BaseReport>.Sort.Ascending(d => d.Date);
            else
                return Builders<BaseReport>.Sort.Descending(d => d.Date);
        }
        else
        {
            return Builders<BaseReport>.Sort.Ascending(report => report.Date);
        }
    }

    public async Task<bool> ReplaceAsync(BaseReport report, CancellationToken token = default)
    {
        var filter = Builders<BaseReport>.Filter.Eq(r => r.Id, report.Id);
        
        var result = await _reportsCollection.ReplaceOneAsync(filter, report, cancellationToken: token);
        
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }
}
