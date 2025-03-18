using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.TokenUtilities;
using Hangfire;
using Hangfire.MySql;
using Services.Implement;
using Services.Interfaces;
using Repositories.UnitOfWork;
using Services.PaymentSetting;
using SharedLibrary.EmailUtilities;
using SkinTime.Helpers;
using System.Security.Cryptography;
using System.Configuration;
using Services.FileSetting;
using Repositories.Implement;
using Repositories.Interface;


namespace SkinTime.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration config
        )
        {// khai báo tất cả các service ở đây => tìm hiểu midderware, tìm hiểu thêm về addscoped vs addtransient vs addsingleton

            // Shared Libraries
            services.AddTransient<ITokenUtilities, TokenUtilities>();
            services.AddTransient<IEmailUtilities, EmailUtilities>();

            // Repositories and Unit of work.
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Business logic (Application) layer services.
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITherapistService, TherapistService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISkinTimeService, SkinTimeService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICache, Cache>();
            services.AddScoped<FileService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddHangfire(hangfireConfig => hangfireConfig
                         .UseStorage(new MySqlStorage(
        config.GetConnectionString("DefaultConnectionMySQL"),
        new MySqlStorageOptions
        {
            QueuePollInterval = TimeSpan.FromSeconds(15),
            JobExpirationCheckInterval = TimeSpan.FromHours(1),
            CountersAggregateInterval = TimeSpan.FromMinutes(5),
            PrepareSchemaIfNecessary = true
        })
    )
);
            services.AddHangfireServer();
            
            services.AddScoped<IStatisticService, StatisticService>();

            // Auto mapper
            services.AddAutoMapper(typeof(Mapping).Assembly);

            // Middlewares

            // Cấu hình ZaloPay từ appsettings.json
            services.Configure<VNPay>(config.GetSection("VNPay"));
            services.AddScoped<VNPay>(sp => sp.GetRequiredService<IOptions<VNPay>>().Value);
            // Cấu hình ZaloPay từ appsettings.json
            services.Configure<ZaloPay>(config.GetSection("ZaloPay"));
            services.AddScoped<ZaloPay>(sp => sp.GetRequiredService<IOptions<ZaloPay>>().Value);
            services.AddScoped<IEventService, EventService>();

            return services;
        }
    }
}
