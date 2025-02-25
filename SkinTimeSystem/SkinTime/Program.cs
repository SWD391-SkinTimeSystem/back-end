
using SkinTime.BLL.Data;
using SkinTime.Extensions;

namespace SkinTime
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            var config = builder.Configuration;

            services.AddDatabaseConfig(config);
            services.AddApplicationServices(config);
            services.ConfigurateAuthenticationMethod(config);
            services.AddSessionService();
            services.AddRedisService(config);
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            var app = builder.Build();

            await app.AddAutoMigrateDatabase();

            if (app.Environment.IsDevelopment())
            {
                await app.SeedDatabase();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowAll");

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}

