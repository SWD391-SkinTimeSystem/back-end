using Cursus.Core.Options.PaymentSetting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SkinTime.BLL.Data;
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
            services.AddAutoMapper(typeof(Mapping).Assembly);
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITransactionService, TransactionService>();
            // Cấu hình VNPay từ appsettings.json
            services.Configure<VNPay>(config.GetSection("VNPay"));
            services.AddScoped<VNPay>(sp => sp.GetRequiredService<IOptions<VNPay>>().Value);

            // Cấu hình ZaloPay từ appsettings.json
            services.Configure<ZaloPay>(config.GetSection("ZaloPay"));
            services.AddScoped<ZaloPay>(sp => sp.GetRequiredService<IOptions<ZaloPay>>().Value);
            return services;
        }
    }
}
