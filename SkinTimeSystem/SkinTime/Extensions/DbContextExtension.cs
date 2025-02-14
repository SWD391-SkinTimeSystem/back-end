using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SkinTime.DAL.Entities;

namespace SkinTime.BLL.Data
{
    public static class DbContextExtension
    {
        public static IServiceCollection AddDatabaseConfig(
            this IServiceCollection services,
            IConfiguration config
        )
        {

           //  Code to use DbContext for MySQL database engine 
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseMySql( 
                config.GetConnectionString("DefaultConnectionMySQL"),
                ServerVersion.AutoDetect(config.GetConnectionString("DefaultConnectionMySQL"))
                ));

            // Code to use DbContext for SQL Server database engine 
            //services.AddDbContext<ApplicationDbContext>(options =>
            //{
            //    options.UseLazyLoadingProxies();
            //    options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
           // });

            return services;

        }



        public static async Task<WebApplication> AddAutoMigrateDatabase(this WebApplication app)
        {// tự chạy migration khi app chạy 
            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            try
            {
                //        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

                //        if (await context.Database.EnsureDeletedAsync())
                //        {
                //            var logger = loggerFactory.CreateLogger<Program>();
                //            logger.LogInformation("Database deleted successfully.");
                //        }

                //        await context.Database.EnsureCreatedAsync();
                //        loggerFactory.CreateLogger<Program>().LogInformation("Database created successfully.");

                var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occur during migration");
            }

            return app;
        }

    }
}
