using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BosquesNalcahue.Application.Models;
using Microsoft.AspNetCore.Http;

namespace BosquesNalcahue.Application.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly string _containerName = "testingpdfs";
    private readonly string connectionString = "DefaultEndpointsProtocol=https;AccountName=bosquesnalcahuestorage;AccountKey=zKMW4ntIt7kNnk86tEkqRk731gnRgjtXAyFLHat690izSWWxfWQCJFATnElr7uy71jXn+K3u3HJA+AStp6ch9Q==;EndpointSuffix=core.windows.net";
    private readonly BlobServiceClient blobServiceClient;

    public BlobStorageService()
    {
        blobServiceClient = new BlobServiceClient(connectionString);
    }

    public Task DeleteBlobAsync(Guid blobId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<FileResponseModel> GetAccessToBlobAsync(Guid blobId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<(Guid, string)> UploadBlobAsync(Stream stream, string contentType, CancellationToken cancellationToken = default)
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

        var blobName = Guid.NewGuid();
        BlobClient blobClient = containerClient.GetBlobClient(blobName.ToString());

        await blobClient.UploadAsync(
            stream, 
            new BlobHttpHeaders { ContentType = contentType}, 
            cancellationToken: cancellationToken);

        return (blobName, blobClient.Uri.ToString());
    }
}
