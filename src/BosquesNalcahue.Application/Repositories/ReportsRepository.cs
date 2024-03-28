using BosquesNalcahue.Application.Entities;
using BosquesNalcahue.Application.Models;
using FluentValidation;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Text.RegularExpressions;

namespace BosquesNalcahue.Application.Repositories;

public class ReportsRepository : IReportsRepository
{
    private readonly IMongoCollection<BaseReport> _reportsCollection;
    private readonly IValidator<GetAllReportsOptions> _optionsValidator;

    public ReportsRepository(IMongoDbOptions mongoDb, IValidator<GetAllReportsOptions> validator)
    {
        MongoClient client = new(mongoDb.Server);
        IMongoDatabase database = client.GetDatabase(mongoDb.Database);
        _reportsCollection = database.GetCollection<BaseReport>(mongoDb.Collection);
        _optionsValidator = validator;
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

    public void FilterDocuments(GetAllReportsOptions options, ref IMongoQueryable<BaseReport> collection)
    {
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
    }

    public async Task<IEnumerable<BaseReport>> GetAllAsync(
        GetAllReportsOptions options, CancellationToken token = default)
    {
        // Validate options
        await _optionsValidator.ValidateAndThrowAsync(options, cancellationToken: token);

        // Get a queryable collection
        var collection = _reportsCollection.AsQueryable();

        // Filtering
        FilterDocuments(options, ref collection);

        // Sorting
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

        
        // Pagination
        collection = collection.Skip((options.Page - 1) * options.PageSize)
                               .Take(options.PageSize);

        return await collection.ToListAsync(cancellationToken: token);
    }

    public async Task<BaseReport> GetByIdAsync(ObjectId id, CancellationToken token = default)
    {
        var filter = Builders<BaseReport>.Filter.Eq(report => report.Id, id);
        var report = await _reportsCollection.Find(filter).FirstOrDefaultAsync(cancellationToken: token);
        return report;
    }

    public async Task<int> GetTotalReports(GetAllReportsOptions options, CancellationToken token = default)
    {
        var collection = _reportsCollection.AsQueryable();

        FilterDocuments(options, ref collection);

        return await collection.CountAsync(cancellationToken: token);
    }

    public async Task<bool> ReplaceAsync(BaseReport report, CancellationToken token = default)
    {
        var filter = Builders<BaseReport>.Filter.Eq(r => r.Id, report.Id);
        
        var result = await _reportsCollection.ReplaceOneAsync(filter, report, cancellationToken: token);
        
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }
}
