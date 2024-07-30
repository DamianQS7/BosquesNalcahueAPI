using BosquesNalcahue.Application.Models;

namespace BosquesNalcahue.Application.Services;

public interface IBlobStorageService
{
    Task<(Guid FileName, string FileUrl)> UploadBlobAsync(Stream stream, string contentType, CancellationToken cancellationToken = default);
    Task<Uri> GetUriToBlobAsync(Guid blobId, CancellationToken cancellationToken = default);
    Task DeleteBlobAsync(Guid blobId, CancellationToken cancellationToken = default);
    
}
