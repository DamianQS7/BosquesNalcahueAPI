using BosquesNalcahue.Application.Entities;
using BosquesNalcahue.Application.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace BosquesNalcahue.Application.Repositories;

public interface IReportsRepository
{    
    Task CreateAsync(BaseReport report, CancellationToken token = default);
    Task<bool> DeleteByIdAsync(ObjectId id, CancellationToken token = default);
    void FilterDocuments(GetAllReportsOptions options, ref IMongoQueryable<BaseReport> collection);
    Task<IEnumerable<BaseReport>> GetAllAsync(GetAllReportsOptions options, CancellationToken token = default);
    Task<BaseReport> GetByIdAsync(ObjectId id, CancellationToken token = default);
    Task<int> GetTotalReports(GetAllReportsOptions options, CancellationToken token = default);
    Task<bool> ReplaceAsync(BaseReport report, CancellationToken token = default);
}
