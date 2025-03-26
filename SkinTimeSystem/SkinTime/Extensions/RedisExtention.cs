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
            string redisConnection = configuration.GetConnectionString("Redis") ?? "localhost:6379";

            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(redisConnection));

            services.AddScoped<IDatabase>(sp =>
                sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase());

            return services;
        }
      
    }
}
