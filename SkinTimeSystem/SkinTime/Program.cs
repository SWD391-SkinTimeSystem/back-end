
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

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            await app.AddAutoMigrateDatabase();
            app.MapControllers();
            app.Run();
        }
    }
}

