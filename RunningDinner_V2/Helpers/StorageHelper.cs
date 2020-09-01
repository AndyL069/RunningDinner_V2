using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RunningDinner.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RunningDinner.Helpers
{
    public static class StorageHelper
    {
        public static bool IsImage(IFormFile file)
        {
            if (file.ContentType.Contains("image", StringComparison.CurrentCulture))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };
            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        public static async Task<List<string>> GetEventPictures(IConfiguration configuration)
        {
            var connectionString = configuration?.GetAzureStorageSettings("ConnectionString");
            var imageContainer = configuration?.GetAzureStorageSettings("ImageContainer");
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            // Create the container and return a container client object
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(imageContainer);

            List<string> eventPictures = new List<string>();
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                if (blobItem.Name.Contains("events", StringComparison.OrdinalIgnoreCase))
                {
                    eventPictures.Add(blobItem.Name);
                }
            }

            return eventPictures;
        }

        public static async Task<Response<BlobContentInfo>> UploadFileToStorage(Stream fileStream, string fileName, IConfiguration configuration)
        {
            var connectionString = configuration?.GetAzureStorageSettings("ConnectionString");
            var imageContainer = configuration?.GetAzureStorageSettings("ImageContainer");
            // Create the blob client.
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            // Create the container and return a container client object
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(imageContainer);
            // Create the blob client
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            // Upload the file
            Response<BlobContentInfo> response = await blobClient.UploadAsync(fileStream);
            return response;
        }

        public static async Task<List<string>> GetThumbNailUrls(IConfiguration configuration)
        {
            List<string> eventPictures;
            List<string> thumbnailUrls = new List<string>();
            var baseUrl = configuration.GetAzureStorageSettings("BaseUrl");
            var imageContainer = configuration.GetAzureStorageSettings("ImageContainer");
            eventPictures = await GetEventPictures(configuration);
            foreach (string image in eventPictures)
            {
                thumbnailUrls.Add(baseUrl + imageContainer + "/" + image);
            }

            return thumbnailUrls;
        }
    }
}
