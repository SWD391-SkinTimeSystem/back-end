using Microsoft.EntityFrameworkCore;
using SkinTime.BLL.Data;
using SkinTime.BLL.Services.QuestionService;
using SkinTime.BLL.Services.SkinTimeService;
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
            services.AddAutoMapper(typeof(Mapping).Assembly);
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<ISkinTimeService, SkinTimeService>();
            return services;
        }
    }
}
