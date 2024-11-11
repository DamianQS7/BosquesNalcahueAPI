﻿using Azure;
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
        var containerClient = serviceClient.GetBlobContainerClient(containerName);

        BlobClient blobClient = containerClient.GetBlobClient(blobId.ToString());

        return await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task<Uri> GetSasUriToBlobAsync(string containerName, string blobId, CancellationToken cancellationToken = default)
    {
        var containerClient = serviceClient.GetBlobContainerClient(containerName);

        BlobClient blobClient = containerClient.GetBlobClient(blobId);

        return CreateBlobSAS(blobClient);
    }

    public async Task<Response<BlobContentInfo>> UploadBlobAsync(string containerName, string fileName, Stream stream, string contentType = "application/pdf", CancellationToken cancellationToken = default)
    {
        var blobName = string.IsNullOrEmpty(fileName) ? Guid.NewGuid().ToString() : fileName;

        var containerClient = serviceClient.GetBlobContainerClient(containerName);

        BlobClient blobClient = containerClient.GetBlobClient(blobName);

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
}
