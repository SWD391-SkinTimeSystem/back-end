using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FileSetting
{
    public class FileService(BlobServiceClient blobServiceClient, IConfiguration configuration)
    {
        private readonly BlobServiceClient _blobServiceClient = blobServiceClient;
        private readonly string _containerName = configuration["AzureStorage:ContainerName"]!;

        public async Task<string> Upload(IFormFile file)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            string newFileName = $"{Guid.NewGuid().ToString()}_{file.FileName}";

            var blobClient = containerClient.GetBlobClient(newFileName);

            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = GetContentType(file.FileName),
            };

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(
                    stream,
                    new BlobUploadOptions { HttpHeaders = blobHttpHeaders }
                );
            }
            return blobClient.Uri.ToString();
        }

        public async Task<bool> Delete(string name)
        {
            var uri = new Uri(name);
            var fileName = Path.GetFileName(uri.LocalPath);
            //create container instance
            var containerInstance = _blobServiceClient.GetBlobContainerClient(_containerName);
            //create blob instance
            var blobInstance = containerInstance.GetBlobClient(fileName);
            var res = await blobInstance.DeleteIfExistsAsync();
            return res.Value;
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" =>
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                _ => "application/octet-stream",
            };
        }
    }
}
