using BosquesNalcahue.Application.Models;

namespace BosquesNalcahue.Application.Services;

public interface IBlobStorageService
{
    Task<Guid> UploadBlobAsync(Stream stream, string contentType, CancellationToken cancellationToken = default);
    Task<Uri> GetSasUriToBlobAsync(Guid blobId, CancellationToken cancellationToken = default);
    Task<bool> DeleteBlobAsync(Guid blobId, CancellationToken cancellationToken = default);
    
}
