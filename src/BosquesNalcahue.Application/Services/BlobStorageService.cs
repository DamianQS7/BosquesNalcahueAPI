using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using BosquesNalcahue.Application.Models;
using Microsoft.Extensions.Options;

namespace BosquesNalcahue.Application.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobStorageConfig blobStorageConfig;
    private readonly BlobServiceClient serviceClient;

    public BlobStorageService(IOptions<BlobStorageConfig> options)
    {
        blobStorageConfig = options.Value;
        serviceClient = new BlobServiceClient(blobStorageConfig.ConnectionString);
    }

    public async Task<bool> DeleteBlobAsync(string containerName, string blobId, CancellationToken cancellationToken = default)
    {
        BlobClient blobClient = GetBlobClient(containerName, blobId.ToString());

        return await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task<byte[]> DownloadBlobAsync(string containerName, string blobId, CancellationToken cancellationToken = default)
    {
        BlobClient blobClient = GetBlobClient(containerName, blobId.ToString());
        
        using var memoryStream = new MemoryStream();

        try
        {
            await blobClient.DownloadToAsync(memoryStream, cancellationToken);
            memoryStream.Position = 0;
            return memoryStream.ToArray();
        }
        catch (RequestFailedException ex)
        {
            throw new FileNotFoundException($"Blob not found: {containerName}/{blobId}", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new TaskCanceledException($"Download cancelled: {containerName}/{blobId}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to download blob: {containerName}/{blobId}", ex);
        }
    }

    public async Task<Uri> GetSasUriToBlobAsync(string containerName, string blobId, CancellationToken cancellationToken = default)
    {
        BlobClient blobClient = GetBlobClient(containerName, blobId);

        return CreateBlobSAS(blobClient);
    }

    public async Task<Response<BlobContentInfo>> UploadBlobAsync(string containerName, string fileName, Stream stream, string contentType = "application/pdf", CancellationToken cancellationToken = default)
    {
        var blobName = string.IsNullOrEmpty(fileName) ? Guid.NewGuid().ToString() : fileName;

        BlobClient blobClient = GetBlobClient(containerName, blobName);

        return await blobClient.UploadAsync(
            stream,
            new BlobHttpHeaders { ContentType = contentType },
            cancellationToken: cancellationToken);
    }

    private static Uri CreateBlobSAS(BlobClient blobClient)
    {
        // Check if BlobContainerClient object has been authorized with Shared Key
        if (blobClient.CanGenerateSasUri)
        {
            // Create a SAS token that's valid for one day
            BlobSasBuilder sasBuilder = new(BlobContainerSasPermissions.Read, DateTimeOffset.UtcNow.AddHours(3))
            {
                BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                BlobName = blobClient.Name,
                Resource = "b"
            };

            Uri sasURI = blobClient.GenerateSasUri(sasBuilder);
            
            return sasURI;
        }
        else
        {
            // Client object is not authorized via Shared Key
            return new Uri(string.Empty);
        }
    }

    private BlobClient GetBlobClient(string containerName, string blobId)
    {
        var containerClient = serviceClient.GetBlobContainerClient(containerName);

        return containerClient.GetBlobClient(blobId);
    }
}
