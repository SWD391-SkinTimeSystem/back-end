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
            services.AddSessionService();
            services.AddRedisService(config);
            services.AddControllers();
            services.AddSignalR();
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
            #region Test hangfire
            app.MapGet("/test-hangfire", () =>
            {
                BackgroundJob.Enqueue(() => Console.WriteLine("Hangfire job chạy ngay lập tức!"));
                return "Job đã được xếp hàng!";
            });

            app.MapGet("/schedule-hangfire", () =>
            {
                BackgroundJob.Schedule(() => Console.WriteLine("Hangfire job chạy sau 5 phút!"), TimeSpan.FromMinutes(5));
                return "Job đã được lập lịch chạy sau 5 phút!";
            });

            app.MapGet("/recurring-hangfire", () =>
            {
                RecurringJob.AddOrUpdate("my-recurring-job",
                    () => Console.WriteLine("Hangfire job chạy mỗi phút!"),
                    Cron.MinuteInterval(1)); // Chạy mỗi phút
                return "Job định kỳ đã được thiết lập!";
            });
            #endregion
            app.UseCors("AllowAll");
            app.UseHangfireDashboard("/hangfire");
            app.MapHub<Chathub>("/chatHub");
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            RecurringJob.AddOrUpdate("startup-job",
                () => Console.WriteLine("Hangfire job chạy mỗi giờ khi ứng dụng khởi động!"),
                Cron.Hourly);
            app.Run();
        }
        public void TestHangfire()
        {
            Console.WriteLine("test hangfire");
        }
    }

}

