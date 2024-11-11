using Azure.Storage.Blobs.Models;
using Azure;

namespace BosquesNalcahue.Application.Services;

public interface IBlobStorageService
{
    Task<Response<BlobContentInfo>> UploadBlobAsync(string containerName, string fileName, Stream stream, string contentType = "application/pdf", CancellationToken cancellationToken = default);
    Task<Uri> GetSasUriToBlobAsync(string containerName, string blobId, CancellationToken cancellationToken = default);
    Task<bool> DeleteBlobAsync(string containerName, string blobId, CancellationToken cancellationToken = default);
    Task<byte[]> DownloadBlobAsync(string containerName, string blobId, CancellationToken cancellationToken = default);
}
