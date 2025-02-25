using Cursus.Core.Options.PaymentSetting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.BLL.Data;
using SkinTime.BLL.Services.AuthenticationService;
using SkinTime.BLL.Services.BookingService;
using SkinTime.BLL.Services.FeedbackService;
using SkinTime.BLL.Services.QuestionService;
using SkinTime.BLL.Services.ScheduleService;
using SkinTime.BLL.Services.SkinTimeService;
using SkinTime.BLL.Services.TherapistService;
using SkinTime.BLL.Services.TransactionService;
using SkinTime.BLL.Services.UserService;
using SkinTime.DAL.Interfaces;
using SkinTime.Helpers;

namespace SkinTime.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration config
        )
        {// khai báo tất cả các service ở đây => tìm hiểu midderware, tìm hiểu thêm về addscoped vs addtransient vs addsingleton
            
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
            services.AddScoped<IFeedbackService,  FeedbackService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
               
                    });
            });

            services.AddScoped<ITransactionService, TransactionService>();
            // Auto mapper
            services.AddAutoMapper(typeof(Mapping).Assembly);

            // Shared Libraries
            services.AddTransient<ITokenUtilities, TokenUtilities>();
            services.AddTransient<IEmailUtilities, EmailUtilities>();

            // Middlewares

            // Cấu hình ZaloPay từ appsettings.json
            services.Configure<ZaloPay>(config.GetSection("ZaloPay"));
            services.AddScoped<ZaloPay>(sp => sp.GetRequiredService<IOptions<ZaloPay>>().Value);
            // Cấu hình VNPay từ appsettings.json
            services.Configure<VNPay>(config.GetSection("VNPay"));
            services.AddScoped<VNPay>(sp => sp.GetRequiredService<IOptions<VNPay>>().Value);
            return services;
        }
    }
}
