﻿using Azure.Storage.Blobs;
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
    private readonly BlobContainerClient containerClient;

    public BlobStorageService(IOptions<BlobStorageConfig> options)
    {
        blobStorageConfig = options.Value;
        serviceClient = new BlobServiceClient(blobStorageConfig.ConnectionString);
        containerClient = serviceClient.GetBlobContainerClient(blobStorageConfig.ContainerName);
    }

    public async Task<bool> DeleteBlobAsync(string blobId, CancellationToken cancellationToken = default)
    {
        BlobClient blobClient = containerClient.GetBlobClient(blobId.ToString());

        return await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task<Uri> GetSasUriToBlobAsync(string blobId, CancellationToken cancellationToken = default)
    {
        BlobClient blobClient = containerClient.GetBlobClient(blobId);

        return CreateBlobSAS(blobClient);
    }

    public async Task UploadBlobAsync(string fileName, Stream stream, CancellationToken cancellationToken = default)
    {
        var blobName = string.IsNullOrEmpty(fileName) ? Guid.NewGuid().ToString() : fileName;

        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.UploadAsync(
            stream,
            new BlobHttpHeaders { ContentType = "application/pdf" },
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
            return null;
        }
    }
}
