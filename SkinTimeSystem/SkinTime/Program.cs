using Hangfire;
using SkinTime.Extensions;
using SkinTime.Hubs;

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
            services.AddRedisService(config);
            services.AddControllers();
            services.AddSignalR().AddNewtonsoftJsonProtocol(); ;
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173") 
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials(); 
                    });
            });

            var app = builder.Build();

            await app.AddAutoMigrateDatabase();

            if (app.Environment.IsDevelopment())
            {
                await app.SeedDatabase();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowAll");
            app.MapHub<NotificationHub>("/notificationHub");
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }

}

