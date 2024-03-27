using BosquesNalcahue.Application.Entities;
using BosquesNalcahue.Application.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BosquesNalcahue.Application.Repositories;

public interface IReportsRepository
{    
    Task CreateAsync(BaseReport report, CancellationToken token = default);
    Task<bool> DeleteByIdAsync(ObjectId id, CancellationToken token = default);
    Task<IEnumerable<BaseReport>> GetAllAsync(FilteringOptions options, CancellationToken token = default);
    Task<BaseReport> GetByIdAsync(ObjectId id, CancellationToken token = default);
    //FilterDefinition<BaseReport> GenerateFilter(FilteringOptions options);
    Task<bool> ReplaceAsync(BaseReport report, CancellationToken token = default);
}
