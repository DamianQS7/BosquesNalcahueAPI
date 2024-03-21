using BosquesNalcahue.Application.Entities;
using BosquesNalcahue.Application.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BosquesNalcahue.Application.Repositories;

public class ReportsRepository : IReportsRepository
{
    private IMongoCollection<BaseReport> _reportsCollection;

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

    public async Task<IEnumerable<BaseReport>> GetAllAsync(CancellationToken token = default)
    {
        var documents = await _reportsCollection.FindAsync(Builders<BaseReport>.Filter.Empty, cancellationToken: token)
                                                .Result
                                                .ToListAsync(cancellationToken: token);
        return documents;
    }

    public async Task<BaseReport> GetByIdAsync(ObjectId id, CancellationToken token = default)
    {
        var filter = Builders<BaseReport>.Filter.Eq(report => report.Id, id);
        var report = await _reportsCollection.Find(filter).FirstOrDefaultAsync(cancellationToken: token);
        return report;
    }

    public async Task<bool> ReplaceAsync(BaseReport report, CancellationToken token = default)
    {
        var filter = Builders<BaseReport>.Filter.Eq(r => r.Id, report.Id);
        
        var result = await _reportsCollection.ReplaceOneAsync(filter, report, cancellationToken: token);
        
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }
}
