﻿using Microsoft.EntityFrameworkCore;
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
using SkinTime.BLL.Services.EventService;
using SkinTime.BLL.Services.UserService;
using SkinTime.DAL.Interfaces;
using SkinTime.Helpers;
using SkinTime.BLL.Services.StatisticService;

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
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITherapistService, TherapistService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISkinTimeService, SkinTimeService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IFeedbackService,  FeedbackService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IStatisticService, StatisticService>();

            // Auto mapper
            services.AddAutoMapper(typeof(Mapping).Assembly);

            // Shared Libraries
            services.AddTransient<ITokenUtilities, TokenUtilities>();
            services.AddTransient<IEmailUtilities, EmailUtilities>();

            // Middlewares

            services.AddScoped<IEventService, EventService>();
            
            return services;
        }
    }
}
