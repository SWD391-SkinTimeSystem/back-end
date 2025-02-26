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

            if (!context.Services.Any())
            {
                var serviceCategory = context.ServiceCategories.ToArray();
                for (int i = 0; i < 20; i++)
                {
                    await context.Services.AddAsync(new()
                    {
                        ServiceName = LoremIpsum(1, 2, 1, 1, 1),
                        Description = LoremIpsum(10, 11, 2, 4, 1),
                        Thumbnail = string.Empty,
                        Duration = 1,
                        Price = random.Next(100000, 2000000),
                        ServiceCategoryID = serviceCategory[random.Next(serviceCategory.Count())].Id,
                    });
                }

                await context.SaveChangesAsync();
            }

            if (!context.ServiceDetails.Any())
            {
                var Services = context.Services.ToArray();

                for (int i = 0; i < 20; i++)
                {
                    var Service = Services[random.Next(Services.Count())];
                    var Step = Service.ServiceDetailNavigation.Count() + 1;
                    var UnitPrice = random.Next(100000, 2000000);
                    await context.ServiceDetails.AddAsync(new()
                    {
                        Name = LoremIpsum(1, 2, 1, 1, 1),
                        Description = LoremIpsum(10, 11, 2, 4, 1),
                        Duration = random.Next(15, 120),
                        UnitPrice = UnitPrice,
                        ServiceID = Service.Id,
                        DateToNextStep = random.Next(1, 20),
                        Step = Step
                    });

                    Service.Price += UnitPrice;
                }

                await context.SaveChangesAsync();
            }

            if (!context.ServiceRecommendation.Any())
            {
                var skinTypes = context.SkinTypes.ToArray();
                var services = context.Services.ToArray();

                for (int i = 0; i < 10; i++)
                {
                    await context.ServiceRecommendation.AddAsync(new()
                    {
                        SkinTypeID = skinTypes[random.Next(skinTypes.Count())].Id,
                        ServiceID = services[random.Next(services.Count())].Id,
                    });
                }

                await context.SaveChangesAsync();
            }

            if (!context.Questions.Any())
            {
                for (int i = 0; i < 10; i++)
                {
                    await context.Questions.AddAsync(new()
                    {
                        Content = LoremIpsum(3, 5, 1, 1, 1),
                        OrderNo = i
                    });
                }

                await context.SaveChangesAsync();
            }

            if (!context.QuestionOptions.Any())
            {
                var questions = context.Questions.ToArray();
                var skinTypes = context.SkinTypes.ToArray();
                for (int i = 0; i < 40; i++)
                {
                    await context.QuestionOptions.AddAsync(new()
                    {
                        Content = LoremIpsum(1, 10, 1, 1, 1),
                        QuestionID = questions[random.Next(questions.Count())].Id,
                        SkinTypeID = skinTypes[random.Next(skinTypes.Count())].Id,
                    });
                }

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
