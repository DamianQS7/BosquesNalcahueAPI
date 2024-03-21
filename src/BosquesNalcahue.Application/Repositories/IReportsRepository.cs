using BosquesNalcahue.Application.Entities;
using MongoDB.Bson;

namespace BosquesNalcahue.Application.Repositories;

public interface IReportsRepository
{    
    Task CreateAsync(BaseReport report, CancellationToken token = default);
    Task<bool> DeleteByIdAsync(ObjectId id, CancellationToken token = default);
    Task<IEnumerable<BaseReport>> GetAllAsync(CancellationToken token = default);
    Task<BaseReport> GetByIdAsync(ObjectId id, CancellationToken token = default);
    Task<bool> ReplaceAsync(BaseReport report, CancellationToken token = default);
}
