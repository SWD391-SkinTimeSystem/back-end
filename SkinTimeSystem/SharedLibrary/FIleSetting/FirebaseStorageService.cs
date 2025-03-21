using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.FIleSetting
{
    public class FirebaseStorageService
    {
        private readonly string _bucketName;
        private readonly StorageClient _storageClient;

        public FirebaseStorageService(IConfiguration configuration)
        {

            _bucketName = configuration["Firebase:BucketName"];
            string credentialPath = configuration["Firebase:CredentialPath"];
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);
            if (string.IsNullOrEmpty(_bucketName) || string.IsNullOrEmpty(credentialPath))
            {
                throw new Exception("Firebase configuration is missing!");
            }

            // Khởi tạo Firebase App nếu chưa có
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(credentialPath)
                });
            }

            _storageClient = StorageClient.Create();
        }

        public async Task<string> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File cannot be null or empty.");
            }

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                await _storageClient.UploadObjectAsync(_bucketName, fileName, file.ContentType, memoryStream);
            }

            return $"https://storage.googleapis.com/{_bucketName}/{fileName}";
        }
    }
}
