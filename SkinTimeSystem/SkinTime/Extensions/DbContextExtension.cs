using System;
using System.Configuration;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum;

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

        public static async Task<WebApplication> SeedDatabase(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            try
            {
                var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                await TrySeed(context, app.Configuration);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "Try seeding data failed");
            }

            return app;
        }

        private static string CreateUserPassword(string password)
        {
            byte[] saltBytes;
            RandomNumberGenerator.Fill(saltBytes = new byte[16]); // Generate new salt each time.
            Rfc2898DeriveBytes hashingFunction = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256);
            byte[] hashedPasswordBytes = hashingFunction.GetBytes(40);
            byte[] savedPasswordHash = new byte[saltBytes.Length + hashedPasswordBytes.Length];
            Array.Copy(saltBytes, 0, savedPasswordHash, 0, 16);
            Array.Copy(hashedPasswordBytes, 0, savedPasswordHash, 16, hashedPasswordBytes.Length);

            return Convert.ToBase64String(savedPasswordHash);
        }

        private static string LoremIpsum(int minWords, int maxWords, int minSentences, int maxSentences, int numParagraphs)
        {
            var words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
                "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
                "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};

            var rand = new Random();
            int numSentences = rand.Next(maxSentences - minSentences)
                + minSentences + 1;
            int numWords = rand.Next(maxWords - minWords) + minWords + 1;

            StringBuilder result = new StringBuilder();

            for (int p = 0; p < numParagraphs; p++)
            {
                for (int s = 0; s < numSentences; s++)
                {
                    for (int w = 0; w < numWords; w++)
                    {
                        if (w > 0) { result.Append(" "); }
                        result.Append(words[rand.Next(words.Length)]);
                    }
                    result.Append(". ");
                }
                result.Append("\n");
            }

            return result.ToString();
        }

        private static async Task TrySeed(ApplicationDbContext context, IConfiguration configuration)
        {
            Random random = new Random();

            if (!context.Users.Any())
            {
                string passwordString = "Password";
                var password = CreateUserPassword(passwordString);
                // These data only used for demonstration purposes.
                await context.Users.AddAsync(new User
                {
                    Username = "admin",
                    Password = CreateUserPassword(configuration.GetValue<string>("Admin:Password")!),
                    Email = configuration.GetValue<string>("Admin:Email")!,
                    Gender = Gender.Other,
                    Role = UserRole.Admin
                });

                await context.Users.AddAsync(new User
                {
                    Username = "manager",
                    FullName = "Tran Nguyen Quoc Viet",
                    Password = password,
                    Email = "sample#example.com",
                    Gender = Gender.Female,
                    Role = UserRole.Manager
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Staff_01",
                    FullName = "Hannah",
                    Avatar = string.Empty,
                    Gender = Gender.Female,
                    Email = "staff01#example.com",
                    Password = password,
                    Phone = "324563447",
                    DateOfBirth = DateOnly.Parse("1980/12/31"),
                    Role = UserRole.Staff,
                });
                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Staff_02",
                    FullName = "Gregorry Hainz",
                    Avatar = string.Empty,
                    Gender = Gender.Female,
                    Email = "example07#gmail.com",
                    Password = password,
                    Phone = "1693837522",
                    DateOfBirth = DateOnly.Parse("2000/05/25"),
                    Role = UserRole.Staff,
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Therapist_1",
                    FullName = "Jacky Chan",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "example04#gmail.com",
                    Password = password,
                    Phone = "9912448336",
                    DateOfBirth = DateOnly.Parse("1997/04/20"),
                    Role = UserRole.Therapist,
                    TherapistNavigation = new Therapist
                    {
                        Id = Guid.NewGuid(),
                        BIO = "This is a description field",
                        ExperienceYears = 1,
                        Status = TherapistStatus.Available,
                    }
                });
                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Therapist_02",
                    FullName = "Nguyen Van Lai",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "example05#gmail.com",
                    Password = password,
                    Phone = "8912448336",
                    DateOfBirth = DateOnly.Parse("1997/04/20"),
                    Role = UserRole.Therapist,
                    TherapistNavigation = new Therapist
                    {
                        Id = Guid.NewGuid(),
                        BIO = "This is therapist 2",
                        ExperienceYears = 5,
                        Status = TherapistStatus.Available,
                    }
                });
                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Therapist_03",
                    FullName = "Harley Ferdinand",
                    Avatar = string.Empty,
                    Gender = Gender.Female,
                    Email = "example_therapist_1#gmail.com",
                    Password = password,
                    Phone = "7429486726",
                    DateOfBirth = DateOnly.Parse("1980/11/30"),
                    Role = UserRole.Therapist,
                    TherapistNavigation = new Therapist
                    {
                        Id = Guid.NewGuid(),
                        BIO = "This is Ms.Harley Ferdinand",
                        ExperienceYears = 1,
                        Status = TherapistStatus.Available,
                    }
                });
                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Therapist_5",
                    FullName = "Tommy Vercetti",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "example_therapist_2#gmail.com",
                    Password = password,
                    Phone = "188928777",
                    DateOfBirth = DateOnly.Parse("1991/11/30"),
                    Role = UserRole.Therapist,
                    TherapistNavigation = new Therapist
                    {
                        Id = Guid.NewGuid(),
                        BIO = "This is Mr.Tommy Vercetti",
                        ExperienceYears = 2,
                        Status = TherapistStatus.Available,
                    }
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_01",
                    FullName = "Un Ascii Key",
                    Avatar = string.Empty,
                    Gender = Gender.Female,
                    Email = "example01#gmail.com",
                    Password = password,
                    Phone = "0194302421",
                    DateOfBirth = DateOnly.Parse("2004/12/02"),
                    Role = UserRole.Customer,
                });
                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_02",
                    FullName = "Jack Teal",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "example02#gmail.com",
                    Password = password,
                    Phone = "0997442823",
                    DateOfBirth = DateOnly.Parse("2004/12/02"),
                    Role = UserRole.Customer,
                });
                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_03",
                    FullName = "User Customer 3",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "example03#gmail.com",
                    Password = password,
                    Phone = "0997442823",
                    DateOfBirth = DateOnly.Parse("2003/08/11"),
                    Role = UserRole.Customer,
                });

                await context.SaveChangesAsync();
            }

            if (!context.SkinTypes.Any())
            {
                string[] skins = { "Normal", "Oily", "Dry", "Combination", "Sensitive", "Mature" };

                foreach (var skin in skins)
                {
                    await context.SkinTypes.AddAsync(new()
                    {
                        Name = skin,
                        Description = LoremIpsum(1, 5, 1, 3, 1)
                    });
                }

                await context.SaveChangesAsync();
            }

            if (!context.ServiceCategories.Any())
            {
                List<string> serviceCategories = new List<string>() { "Facial", "Body", "Microdermabrasion", "Chemical Peels", "Laser Skin Resurfacing", "Dermaplaning", "Hydrafacial" };
                foreach (string category in serviceCategories)
                {
                    await context.ServiceCategories.AddAsync(new ServiceCategory
                    {
                        Id = Guid.NewGuid(),
                        Name = category,
                        Status = ServiceCategoryStatus.Enabled,
                    });
                }

                await context.SaveChangesAsync();
            }

            // Kiểm tra và thêm dữ liệu cho Services
            if (!context.Services.Any())
            {
                var serviceCategory = context.ServiceCategories.ToArray();

                await context.Services.AddAsync(new()
                {
                    ServiceName = "Dịch vụ chăm sóc da cơ bản",
                    Description = "Làm sạch da và cung cấp độ ẩm cần thiết cho làn da khỏe mạnh.",
                    Thumbnail = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBYYFhUjvNhBJ9SwQTv8X3PyQVtd-EejwOMQ&s",
                    Duration = 1,
                    Price = 500000,
                    ServiceCategoryID = serviceCategory[0].Id
                });

                await context.Services.AddAsync(new()
                {
                    ServiceName = "Dịch vụ trị mụn chuyên sâu",
                    Description = "Điều trị mụn hiệu quả, giảm viêm và ngăn ngừa mụn tái phát.",
                    Thumbnail = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBYYFhUjvNhBJ9SwQTv8X3PyQVtd-EejwOMQ&s",
                    Duration = 1,
                    Price = 1200000,
                    ServiceCategoryID = serviceCategory[1].Id
                });

                await context.Services.AddAsync(new()
                {
                    ServiceName = "Dịch vụ làm trắng da",
                    Description = "Giúp da sáng mịn và đều màu hơn.",
                    Thumbnail = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBYYFhUjvNhBJ9SwQTv8X3PyQVtd-EejwOMQ&s",
                    Duration = 2,
                    Price = 1500000,
                    ServiceCategoryID = serviceCategory[0].Id
                });

                await context.Services.AddAsync(new()
                {
                    ServiceName = "Dịch vụ trẻ hóa da",
                    Description = "Giảm nếp nhăn và tăng độ đàn hồi cho da.",
                    Thumbnail = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBYYFhUjvNhBJ9SwQTv8X3PyQVtd-EejwOMQ&s",
                    Duration = 1,
                    Price = 2000000,
                    ServiceCategoryID = serviceCategory[1].Id
                });

                await context.Services.AddAsync(new()
                {
                    ServiceName = "Dịch vụ tẩy tế bào chết",
                    Description = "Loại bỏ lớp da chết, giúp da thông thoáng.",
                    Thumbnail = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBYYFhUjvNhBJ9SwQTv8X3PyQVtd-EejwOMQ&s",
                    Duration = 1,
                    Price = 400000,
                    ServiceCategoryID = serviceCategory[0].Id
                });

                await context.Services.AddAsync(new()
                {
                    ServiceName = "Dịch vụ massage mặt",
                    Description = "Thư giãn và cải thiện tuần hoàn máu.",
                    Thumbnail = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBYYFhUjvNhBJ9SwQTv8X3PyQVtd-EejwOMQ&s",
                    Duration = 1,
                    Price = 600000,
                    ServiceCategoryID = serviceCategory[1].Id
                });

                await context.Services.AddAsync(new()
                {
                    ServiceName = "Dịch vụ chăm sóc da cao cấp",
                    Description = "Kết hợp nhiều liệu pháp cho làn da hoàn hảo.",
                    Thumbnail = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBYYFhUjvNhBJ9SwQTv8X3PyQVtd-EejwOMQ&s",
                    Duration = 2,
                    Price = 2500000,
                    ServiceCategoryID = serviceCategory[0].Id
                });

                await context.Services.AddAsync(new()
                {
                    ServiceName = "Dịch vụ điều trị nám",
                    Description = "Giảm thâm nám và cải thiện sắc tố da.",
                    Thumbnail = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBYYFhUjvNhBJ9SwQTv8X3PyQVtd-EejwOMQ&s",
                    Duration = 1,
                    Price = 1800000,
                    ServiceCategoryID = serviceCategory[1].Id
                });

                await context.Services.AddAsync(new()
                {
                    ServiceName = "Dịch vụ dưỡng da sâu",
                    Description = "Cung cấp dưỡng chất cần thiết cho da.",
                    Thumbnail = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBYYFhUjvNhBJ9SwQTv8X3PyQVtd-EejwOMQ&s",
                    Duration = 1,
                    Price = 900000,
                    ServiceCategoryID = serviceCategory[0].Id
                });

                await context.Services.AddAsync(new()
                {
                    ServiceName = "Dịch vụ chăm sóc vùng mắt",
                    Description = "Giảm quầng thâm và nếp nhăn quanh mắt.",
                    Thumbnail = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBYYFhUjvNhBJ9SwQTv8X3PyQVtd-EejwOMQ&s",
                    Duration = 1,
                    Price = 700000,
                    ServiceCategoryID = serviceCategory[1].Id
                });

                await context.SaveChangesAsync();
            }

            // Kiểm tra và thêm dữ liệu cho ServiceDetails
            if (!context.ServiceDetails.Any())
            {
                var services = context.Services.ToArray();

                var service1 = services[0];
                await context.ServiceDetails.AddAsync(new() { Name = "Làm sạch da", Description = "Loại bỏ bụi bẩn và dầu thừa.", Duration = 30, UnitPrice = 300000, ServiceID = service1.Id, DateToNextStep = 5, Step = 1 });
                service1.Price += 300000;

                var service2 = services[1];
                await context.ServiceDetails.AddAsync(new() { Name = "Điều trị mụn", Description = "Sử dụng công nghệ trị mụn.", Duration = 60, UnitPrice = 800000, ServiceID = service2.Id, DateToNextStep = 7, Step = 1 });
                service2.Price += 800000;

                var service3 = services[2];
                await context.ServiceDetails.AddAsync(new() { Name = "Dưỡng trắng", Description = "Cải thiện độ sáng da.", Duration = 45, UnitPrice = 900000, ServiceID = service3.Id, DateToNextStep = 10, Step = 1 });
                service3.Price += 900000;

                var service4 = services[3];
                await context.ServiceDetails.AddAsync(new() { Name = "Trẻ hóa", Description = "Kích thích collagen.", Duration = 60, UnitPrice = 1200000, ServiceID = service4.Id, DateToNextStep = 8, Step = 1 });
                service4.Price += 1200000;

                var service5 = services[4];
                await context.ServiceDetails.AddAsync(new() { Name = "Tẩy tế bào", Description = "Làm sạch sâu.", Duration = 20, UnitPrice = 200000, ServiceID = service5.Id, DateToNextStep = 3, Step = 1 });
                service5.Price += 200000;

                var service6 = services[5];
                await context.ServiceDetails.AddAsync(new() { Name = "Massage", Description = "Thư giãn cơ mặt.", Duration = 30, UnitPrice = 350000, ServiceID = service6.Id, DateToNextStep = 5, Step = 1 });
                service6.Price += 350000;

                var service7 = services[6];
                await context.ServiceDetails.AddAsync(new() { Name = "Chăm sóc sâu", Description = "Đắp mặt nạ cao cấp.", Duration = 60, UnitPrice = 1500000, ServiceID = service7.Id, DateToNextStep = 12, Step = 1 });
                service7.Price += 1500000;

                var service8 = services[7];
                await context.ServiceDetails.AddAsync(new() { Name = "Trị nám", Description = "Giảm sắc tố.", Duration = 45, UnitPrice = 1000000, ServiceID = service8.Id, DateToNextStep = 7, Step = 1 });
                service8.Price += 1000000;

                var service9 = services[8];
                await context.ServiceDetails.AddAsync(new() { Name = "Dưỡng chất", Description = "Cung cấp vitamin.", Duration = 40, UnitPrice = 500000, ServiceID = service9.Id, DateToNextStep = 6, Step = 1 });
                service9.Price += 500000;

                var service10 = services[9];
                await context.ServiceDetails.AddAsync(new() { Name = "Chăm sóc mắt", Description = "Giảm quầng thâm.", Duration = 30, UnitPrice = 400000, ServiceID = service10.Id, DateToNextStep = 4, Step = 1 });
                service10.Price += 400000;

                await context.SaveChangesAsync();
            }

            // Kiểm tra và thêm dữ liệu cho ServiceRecommendation
            if (!context.ServiceRecommendation.Any())
            {
                var skinTypes = context.SkinTypes.ToArray();
                var services = context.Services.ToArray();

                await context.ServiceRecommendation.AddAsync(new() { SkinTypeID = skinTypes[0].Id, ServiceID = services[0].Id });
                await context.ServiceRecommendation.AddAsync(new() { SkinTypeID = skinTypes[1].Id, ServiceID = services[1].Id });
                await context.ServiceRecommendation.AddAsync(new() { SkinTypeID = skinTypes[2].Id, ServiceID = services[2].Id });
                await context.ServiceRecommendation.AddAsync(new() { SkinTypeID = skinTypes[0].Id, ServiceID = services[3].Id });
                await context.ServiceRecommendation.AddAsync(new() { SkinTypeID = skinTypes[1].Id, ServiceID = services[4].Id });
                await context.ServiceRecommendation.AddAsync(new() { SkinTypeID = skinTypes[2].Id, ServiceID = services[5].Id });
                await context.ServiceRecommendation.AddAsync(new() { SkinTypeID = skinTypes[0].Id, ServiceID = services[6].Id });
                await context.ServiceRecommendation.AddAsync(new() { SkinTypeID = skinTypes[1].Id, ServiceID = services[7].Id });
                await context.ServiceRecommendation.AddAsync(new() { SkinTypeID = skinTypes[2].Id, ServiceID = services[8].Id });
                await context.ServiceRecommendation.AddAsync(new() { SkinTypeID = skinTypes[0].Id, ServiceID = services[9].Id });

                await context.SaveChangesAsync();
            }

            // Kiểm tra và thêm dữ liệu cho Questions
            if (!context.Questions.Any())
            {
                await context.Questions.AddAsync(new() { Content = "Da bạn thuộc loại nào?", OrderNo = 1 });
                await context.Questions.AddAsync(new() { Content = "Bạn có thường xuyên bị mụn không?", OrderNo = 2 });
                await context.Questions.AddAsync(new() { Content = "Da bạn có nhạy cảm không?", OrderNo = 3 });
                await context.Questions.AddAsync(new() { Content = "Bạn có bị nám da không?", OrderNo = 4 });
                await context.Questions.AddAsync(new() { Content = "Da bạn có khô không?", OrderNo = 5 });
                await context.Questions.AddAsync(new() { Content = "Bạn có cần làm sáng da không?", OrderNo = 6 });
                await context.Questions.AddAsync(new() { Content = "Bạn có thường xuyên makeup không?", OrderNo = 7 });
                await context.Questions.AddAsync(new() { Content = "Da bạn có dầu nhiều không?", OrderNo = 8 });
                await context.Questions.AddAsync(new() { Content = "Bạn có bị quầng thâm mắt không?", OrderNo = 9 });
                await context.Questions.AddAsync(new() { Content = "Bạn muốn trẻ hóa da không?", OrderNo = 10 });

                await context.SaveChangesAsync();
            }

            // Kiểm tra và thêm dữ liệu cho QuestionOptions
            if (!context.QuestionOptions.Any())
            {
                var questions = context.Questions.ToArray();
                var skinTypes = context.SkinTypes.ToArray();

                await context.QuestionOptions.AddAsync(new() { Content = "Da dầu", QuestionID = questions[0].Id, SkinTypeID = skinTypes[0].Id });
                await context.QuestionOptions.AddAsync(new() { Content = "Có, rất thường xuyên", QuestionID = questions[1].Id, SkinTypeID = skinTypes[1].Id });
                await context.QuestionOptions.AddAsync(new() { Content = "Có, rất nhạy cảm", QuestionID = questions[2].Id, SkinTypeID = skinTypes[2].Id });
                await context.QuestionOptions.AddAsync(new() { Content = "Có, bị nám nhiều", QuestionID = questions[3].Id, SkinTypeID = skinTypes[0].Id });
                await context.QuestionOptions.AddAsync(new() { Content = "Có, da rất khô", QuestionID = questions[4].Id, SkinTypeID = skinTypes[1].Id });
                await context.QuestionOptions.AddAsync(new() { Content = "Có, muốn sáng hơn", QuestionID = questions[5].Id, SkinTypeID = skinTypes[2].Id });
                await context.QuestionOptions.AddAsync(new() { Content = "Có, makeup hàng ngày", QuestionID = questions[6].Id, SkinTypeID = skinTypes[0].Id });
                await context.QuestionOptions.AddAsync(new() { Content = "Có, dầu nhiều", QuestionID = questions[7].Id, SkinTypeID = skinTypes[1].Id });
                await context.QuestionOptions.AddAsync(new() { Content = "Có, quầng thâm rõ", QuestionID = questions[8].Id, SkinTypeID = skinTypes[2].Id });
                await context.QuestionOptions.AddAsync(new() { Content = "Có, muốn trẻ hóa", QuestionID = questions[9].Id, SkinTypeID = skinTypes[0].Id });

                await context.SaveChangesAsync();
            }

            if (!context.Events.Any())
            {
                await context.Events.AddAsync(new()
                {
                    Name = "Hội thảo du lịch nghĩ dưỡng chăm sóc bản thân 2025",
                    Description = "Đây là nội dung mẫu được đánh máy nhằm mục đích tạo văn bản mẫu",
                    Capacity = 100,
                    EventDate = DateOnly.Parse("2025/11/20"),
                    Location = "Hall Alpha",
                    TicketPrice = 100000,
                    TimeStart = TimeOnly.Parse("12:00:00"),
                    TimeEnd = TimeOnly.Parse("14:00:00"),
                });

                await context.Events.AddAsync(new()
                {
                    Name = "Chuyên đề về lợi ích của hệ thống chăm sóc da.",
                    Description = "Đây là nội dung mẫu được đánh máy nhằm mục đích tạo văn bản mẫu",
                    Capacity = 250,
                    EventDate = DateOnly.Parse("2025/11/22"),
                    Location = "Hall Alpha",
                    TicketPrice = 150000,
                    TimeStart = TimeOnly.Parse("07:00:00"),
                    TimeEnd = TimeOnly.Parse("09:00:00"),
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
