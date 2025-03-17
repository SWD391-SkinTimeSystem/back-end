using Azure.Storage.Blobs;
using System.Text.Json;

namespace SkinTime.Extensions
{
    public static class BlobServiceExtension
    {
        public static IServiceCollection AddBlobService(
           this IServiceCollection services,
           IConfiguration config
       )
        {
            var blobConnectionString = config["AzureStorage:BlobConnectionString"];
            services.AddScoped(_ => new BlobServiceClient(blobConnectionString));
            return services;
        }
    }
}
 