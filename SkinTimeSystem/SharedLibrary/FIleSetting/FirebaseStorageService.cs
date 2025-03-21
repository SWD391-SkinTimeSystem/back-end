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
            // Đọc cấu hình từ appsettings.json
            _bucketName = configuration["Firebase:BucketName"];
            string credentialPath = configuration["Firebase:CredentialPath"];

            // Kiểm tra nếu chưa cấu hình đúng
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

          //  _storageClient = StorageClient.Create(credential);
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var storageObject = await _storageClient.UploadObjectAsync(_bucketName, fileName, contentType, fileStream);
            return $"https://storage.googleapis.com/{_bucketName}/{storageObject.Name}";
        }
        public async Task<string> Upload(IFormFile file)
        {
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
