using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
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

    public async Task<Uri> GetUriToBlobAsync(Guid blobId, CancellationToken cancellationToken = default)
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

        BlobClient blobClient = containerClient.GetBlobClient(blobId.ToString());

        return await CreateBlobSAS(blobClient);
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

    private static async Task<Uri> CreateBlobSAS(BlobClient blobClient, string storedPolicyName = null)
    {
        // Check if BlobContainerClient object has been authorized with Shared Key
        if (blobClient.CanGenerateSasUri)
        {
            // Create a SAS token that's valid for one day
            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                BlobName = blobClient.Name,
                Resource = "b"
            };

            if (storedPolicyName == null)
            {
                sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(3);
                sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);
            }
            else
            {
                sasBuilder.Identifier = storedPolicyName;
            }

            Uri sasURI = blobClient.GenerateSasUri(sasBuilder);

            return sasURI;
        }
        else
        {
            // Client object is not authorized via Shared Key
            return null;
        }
    }
}
