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
        await _reportsCollection.InsertOneAsync(report, cancellationToken: token);
    }

    public Task DeleteByIdAsync(ObjectId id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<BaseReport>> GetAllAsync(CancellationToken token = default)
    {
        var documents = await _reportsCollection.FindAsync(Builders<BaseReport>.Filter.Empty, cancellationToken: token)
                                                .Result
                                                .ToListAsync(cancellationToken: token);
        return documents;
    }
}
