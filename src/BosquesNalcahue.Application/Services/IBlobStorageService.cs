using BosquesNalcahue.Application.Models;

namespace BosquesNalcahue.Application.Services;

public interface IBlobStorageService
{
    Task UploadBlobAsync(string fileName, Stream stream, CancellationToken cancellationToken = default);
    Task<Uri> GetSasUriToBlobAsync(string blobId, CancellationToken cancellationToken = default);
    Task<bool> DeleteBlobAsync(string blobId, CancellationToken cancellationToken = default);
    
}
