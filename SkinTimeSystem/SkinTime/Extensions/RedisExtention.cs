using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Text.Json;
using System.Threading.Tasks;

namespace SkinTime.Extensions
{
    public static class RedisExtension
    {
        public static IServiceCollection AddRedisService(this IServiceCollection services, IConfiguration configuration)
        {
            // Đọc connection string từ appsettings.json
            string redisConnection = configuration.GetConnectionString("Redis") ?? "localhost:6379";

            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(redisConnection));

            services.AddScoped<IDatabase>(sp =>
                sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase());

            return services;
        }

        public static async Task SetAsync<T>(this IDatabase redis, string key, T value, TimeSpan? expiry = null)
        {
            string json = JsonSerializer.Serialize(value);
            await redis.StringSetAsync(key, json, expiry);
        }

        public static async Task<T?> GetAsync<T>(this IDatabase redis, string key)
        {
            string json = await redis.StringGetAsync(key);
            return string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json);
        }

        public static async Task<bool> DeleteAsync(this IDatabase redis, string key)
        {
            return await redis.KeyDeleteAsync(key);
        }
    }
}
