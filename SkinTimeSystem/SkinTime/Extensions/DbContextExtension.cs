using System;
using System.Configuration;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using BusinessObject.Entities;
using BusinessObject.Enum;
using BusinessObject.EventEnums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Repositories.Data;

namespace SkinTime.Extensions
{
    public static class DbContextExtension
    {
        public static IServiceCollection AddDatabaseConfig(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseMySql(
                config.GetConnectionString("DefaultConnectionMySQL"),
                ServerVersion.AutoDetect(config.GetConnectionString("DefaultConnectionMySQL"))
            ));

            return services;
        }

        public static async Task<WebApplication> AddAutoMigrateDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            try
            {
                var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred during migration");
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
            RandomNumberGenerator.Fill(saltBytes = new byte[16]);
            Rfc2898DeriveBytes hashingFunction = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256);
            byte[] hashedPasswordBytes = hashingFunction.GetBytes(40);
            byte[] savedPasswordHash = new byte[saltBytes.Length + hashedPasswordBytes.Length];
            Array.Copy(saltBytes, 0, savedPasswordHash, 0, 16);
            Array.Copy(hashedPasswordBytes, 0, savedPasswordHash, 16, hashedPasswordBytes.Length);

            return Convert.ToBase64String(savedPasswordHash);
        }


        private static async Task TrySeed(ApplicationDbContext context, IConfiguration configuration)
        {
            Random random = new Random();

            // 1. Seed Users
            if (!context.Users.Any())
            {
                string defaultPassword = "Password";
                var hashedPassword = CreateUserPassword(defaultPassword);

                // Admin
                await context.Users.AddAsync(new User
                {
                    Username = "Admin",
                    Password = CreateUserPassword(configuration.GetValue<string>("Admin:Password") ?? "Admin123"),
                    Email = configuration.GetValue<string>("Admin:Email") ?? "admin@example.com",
                    Gender = Gender.Male,
                    Role = UserRole.Admin
                });

                // Manager
                await context.Users.AddAsync(new User
                {
                    Username = "Manager",
                    FullName = "Tran Nguyen Quoc Viet",
                    Password = hashedPassword,
                    Email = "sample@example.com",
                    Gender = Gender.Female,
                    Role = UserRole.Manager
                });

                // Staff
                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Staff_01",
                    FullName = "Anh Thư",
                    Avatar = string.Empty,
                    Gender = Gender.Female,
                    Email = "staff01@gmail.com",
                    Password = hashedPassword,
                    Phone = "324563447",
                    DateOfBirth = DateOnly.Parse("1980/12/31"),
                    Role = UserRole.Staff
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Staff_02",
                    FullName = "Bảo Ngọc",
                    Avatar = string.Empty,
                    Gender = Gender.Female,
                    Email = "example07@gmail.com",
                    Password = hashedPassword,
                    Phone = "1693837522",
                    DateOfBirth = DateOnly.Parse("2000/05/25"),
                    Role = UserRole.Staff
                });

                // Therapists
                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Therapist_1",
                    FullName = "Gia Huy",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "example04@gmail.com",
                    Password = hashedPassword,
                    Phone = "9912448336",
                    DateOfBirth = DateOnly.Parse("1997/04/20"),
                    Role = UserRole.Therapist,
                    TherapistNavigation = new Therapist
                    {
                        Id = Guid.NewGuid(),
                        BIO = "This is a description field",
                        ExperienceYears = 1,
                        Status = TherapistStatus.Available
                    }
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Therapist_02",
                    FullName = "Nguyen Van Lai",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "example05@gmail.com",
                    Password = hashedPassword,
                    Phone = "8912448336",
                    DateOfBirth = DateOnly.Parse("1997/04/20"),
                    Role = UserRole.Therapist,
                    TherapistNavigation = new Therapist
                    {
                        Id = Guid.NewGuid(),
                        BIO = "This is therapist 2",
                        ExperienceYears = 5,
                        Status = TherapistStatus.Available
                    }
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Therapist_03",
                    FullName = "Khánh Linh",
                    Avatar = string.Empty,
                    Gender = Gender.Female,
                    Email = "example_therapist_1@gmail.com",
                    Password = hashedPassword,
                    Phone = "7429486726",
                    DateOfBirth = DateOnly.Parse("1980/11/30"),
                    Role = UserRole.Therapist,
                    TherapistNavigation = new Therapist
                    {
                        Id = Guid.NewGuid(),
                        BIO = "This is Ms. Harley Ferdinand",
                        ExperienceYears = 1,
                        Status = TherapistStatus.Available
                    }
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Therapist_5",
                    FullName = "Đức Minh",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "example_therapist_2@gmail.com",
                    Password = hashedPassword,
                    Phone = "188928777",
                    DateOfBirth = DateOnly.Parse("1991/11/30"),
                    Role = UserRole.Therapist,
                    TherapistNavigation = new Therapist
                    {
                        Id = Guid.NewGuid(),
                        BIO = "This is Mr. Tommy Vercetti",
                        ExperienceYears = 2,
                        Status = TherapistStatus.Available
                    }
                });

                // Customers
                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_01",
                    FullName = "Tuấn Kiệt",
                    Avatar = string.Empty,
                    Gender = Gender.Female,
                    Email = "example01@gmail.com",
                    Password = hashedPassword,
                    Phone = "0194302421",
                    DateOfBirth = DateOnly.Parse("2004/12/02"),
                    Role = UserRole.Customer
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_02",
                    FullName = "Ngọc Sơn",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "example02@gmail.com",
                    Password = hashedPassword,
                    Phone = "0997442823",
                    DateOfBirth = DateOnly.Parse("2004/12/02"),
                    Role = UserRole.Customer
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_03",
                    FullName = "Hữu Phước",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "example03@gmail.com",
                    Password = hashedPassword,
                    Phone = "0997442823",
                    DateOfBirth = DateOnly.Parse("2003/08/11"),
                    Role = UserRole.Customer
                });

                await context.SaveChangesAsync();
            }

            // 2. Seed ServiceCategories
            if (!context.ServiceCategories.Any())
            {
                var categories = new List<ServiceCategory>
        {
            new ServiceCategory { Id = Guid.NewGuid(), Name = "Massage Kỹ Thuật Cao", Status = ServiceCategoryStatus.Enabled },
            new ServiceCategory { Id = Guid.NewGuid(), Name = "Chăm Sóc Da Mặt", Status = ServiceCategoryStatus.Enabled },
            new ServiceCategory { Id = Guid.NewGuid(), Name = "Chăm Sóc Cơ Thể", Status = ServiceCategoryStatus.Enabled },
            new ServiceCategory { Id = Guid.NewGuid(), Name = "Dịch Vụ Tẩy Lông", Status = ServiceCategoryStatus.Enabled },
            new ServiceCategory { Id = Guid.NewGuid(), Name = "Xông Hơi", Status = ServiceCategoryStatus.Enabled }
        };

                await context.ServiceCategories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            // 3. Seed SkinTypes
            if (!context.SkinTypes.Any())
            {
                var skinTypes = new[]
                {
            new SkinType { Name = "Da khô", Description = "Da khô có tuyến bã nhờn hoạt động kém, dễ bị bong tróc và thiếu ẩm..." },
            new SkinType { Name = "Da dầu", Description = "Da dầu có tuyến bã nhờn hoạt động mạnh, dễ bị bóng nhờn và nổi mụn..." },
            new SkinType { Name = "Da hỗn hợp", Description = "Da hỗn hợp có vùng chữ T dầu và các vùng khác khô hoặc thường..." },
            new SkinType { Name = "Da nhạy cảm", Description = "Da nhạy cảm dễ bị kích ứng với các yếu tố môi trường và mỹ phẩm..." },
            new SkinType { Name = "Da thường", Description = "Da thường cân bằng giữa dầu và độ ẩm, ít gặp vấn đề..." }
        };

                await context.SkinTypes.AddRangeAsync(skinTypes);
                await context.SaveChangesAsync();
            }

            // 4. Seed Services
            if (!context.Services.Any())
            {
                var serviceCategories = context.ServiceCategories.ToArray();
                if (serviceCategories.Length < 5)
                {
                    throw new Exception("Không đủ ServiceCategories để seeding Services. Cần ít nhất 5 danh mục.");
                }

                var services = new List<Service>
        {
            // Da khô
            new Service { ServiceName = "Dịch vụ dưỡng ẩm sâu", Description = "Cung cấp dưỡng chất và khóa ẩm lâu dài cho da khô.", Thumbnail = "https://easysalon.vn/wp-content/uploads/2021/04/bang-gia-dich-vu-Spa-2.jpg", Duration = 1, Price = 800000, ServiceCategoryID = serviceCategories[0].Id },
            new Service { ServiceName = "Dịch vụ tái tạo da với mặt nạ collagen", Description = "Giảm bong tróc và tăng độ đàn hồi.", Thumbnail = "https://cdn.dealtoday.vn/img/s800x400/0341ba775a40443d85e685ef19c790ab.jpg?sign=OVhemMvb6L5yOXMFOsSjxw", Duration = 1, Price = 1200000, ServiceCategoryID = serviceCategories[0].Id },
            new Service { ServiceName = "Dịch vụ làm sạch nhẹ nhàng", Description = "Loại bỏ bụi bẩn mà không làm khô da.", Thumbnail = "https://cdn.dealtoday.vn/img/c280x280/LBelle-Beauty-Lay-nhan-mun-va-dien-di-lanh-phuc-hoi-avt_17092024160739.jpg?sign=cC_ZykqWSGqY2KOLsTW8IQ", Duration = 1, Price = 500000, ServiceCategoryID = serviceCategories[0].Id },
            new Service { ServiceName = "Dịch vụ trẻ hóa với vitamin E", Description = "Cải thiện độ căng bóng và chống oxy hóa.", Thumbnail = "https://benhvienthammynaman.com/wp-content/uploads/2023/06/truyen-vitamin-tre-hoa-da-mat-1.jpg", Duration = 1, Price = 1500000, ServiceCategoryID = serviceCategories[0].Id },

            // Da dầu
            new Service { ServiceName = "Dịch vụ kiểm soát nhờn", Description = "Giảm tiết dầu và se khít lỗ chân lông.", Thumbnail = "https://o2skin.vn/wp-content/uploads/2024/05/hinh-anh-gioi-thieu-dich-vu-mat-na-dieu-tri-mun-va-kiem-soat-nhon-3.png", Duration = 1, Price = 900000, ServiceCategoryID = serviceCategories[1].Id },
            new Service { ServiceName = "Dịch vụ trị mụn chuyên sâu", Description = "Giảm viêm và ngăn ngừa mụn tái phát.", Thumbnail = "https://anabelspa.vn/wp-content/uploads/2023/06/dieu-tri-mun-chuyen-sau-500x467.jpg", Duration = 1, Price = 1200000, ServiceCategoryID = serviceCategories[1].Id },
            new Service { ServiceName = "Dịch vụ tẩy tế bào chết hóa học", Description = "Loại bỏ tế bào chết và kiểm soát dầu thừa.", Thumbnail = "https://www.elle.vn/app/uploads/2020/08/13/411653/da-dau-1.jpg", Duration = 1, Price = 800000, ServiceCategoryID = serviceCategories[1].Id },
            new Service { ServiceName = "Dịch vụ làm sạch sâu với than hoạt tính", Description = "Hút nhờn và giảm mụn đầu đen.", Thumbnail = "https://haianhspa.com.vn/wp-content/uploads/2020/03/IMG_8937-1536x1024.jpg", Duration = 1, Price = 1000000, ServiceCategoryID = serviceCategories[1].Id },

            // Da hỗn hợp
            new Service { ServiceName = "Dịch vụ cân bằng độ ẩm vùng chữ T", Description = "Giảm nhờn vùng trán và dưỡng ẩm vùng má.", Thumbnail = "https://bloganchoi.com/wp-content/uploads/2016/09/vung-chu-t-la-gi.jpg", Duration = 1, Price = 1000000, ServiceCategoryID = serviceCategories[2].Id },
            new Service { ServiceName = "Dịch vụ trị mụn cục bộ", Description = "Đặc trị mụn ở vùng trán và cằm.", Thumbnail = "https://blissbeauty.vn/wp-content/uploads/2023/03/7.png", Duration = 1, Price = 1200000, ServiceCategoryID = serviceCategories[2].Id },
            new Service { ServiceName = "Dịch vụ làm sạch 2 bước", Description = "Làm sạch dầu vùng chữ T và giữ ẩm vùng má.", Thumbnail = "https://linhtranspa.com/wp-content/uploads/2021/05/dich-vu-cham-soc-da-mat-chuyen-sau-2.jpg", Duration = 1, Price = 800000, ServiceCategoryID = serviceCategories[2].Id },
            new Service { ServiceName = "Dịch vụ tẩy da chết enzyme", Description = "Loại bỏ tế bào chết nhẹ nhàng và không gây khô da.", Thumbnail = "https://thammymisstram.vn/wp-content/uploads/2021/08/tay-da-chet-bang-enzyme.jpg", Duration = 1, Price = 900000, ServiceCategoryID = serviceCategories[2].Id },
            new Service { ServiceName = "Dịch vụ trẻ hóa với vitamin E", Description = "Cải thiện độ căng bóng và chống oxy hóa.", Thumbnail = "https://https://5.imimg.com/data5/SELLER/Default/2021/3/GU/TE/RL/31552095/ladies-facial-services-1000x1000.jpg", Duration = 60, Price = 1500000, ServiceCategoryID = serviceCategories[0].Id, ServiceDetailNavigation = new List<ServiceDetail> { new ServiceDetail { Step = 1, Name = "Làm sạch da", Duration = 15, Description = "Làm sạch sâu giúp loại bỏ bụi bẩn và dầu thừa.", DateToNextStep = 3 }, new ServiceDetail { Step = 2, Name = "Dưỡng da với vitamin E", Duration = 30, Description = "Cung cấp dưỡng chất giúp da căng bóng và đàn hồi.", DateToNextStep = 3 }, new ServiceDetail { Step = 3, Name = "Khóa ẩm", Duration = 15, Description = "Tăng cường độ ẩm giúp da mềm mại và tươi trẻ.", DateToNextStep = 0} } }


        };

                await context.Services.AddRangeAsync(services);
                await context.SaveChangesAsync();
            }

            // 5. Seed ServiceRecommendations
            var skinTypess = await context.SkinTypes.ToListAsync();
            var servicess = await context.Services.ToListAsync();

            skinTypess[0].Services.Add(servicess[0]);
            skinTypess[1].Services.Add(servicess[1]);
            skinTypess[2].Services.Add(servicess[2]);
            skinTypess[0].Services.Add(servicess[3]);
            skinTypess[1].Services.Add(servicess[4]);
            skinTypess[2].Services.Add(servicess[5]);
            skinTypess[0].Services.Add(servicess[6]);
            skinTypess[1].Services.Add(servicess[7]);
            skinTypess[2].Services.Add(servicess[8]);
            skinTypess[0].Services.Add(servicess[9]);

            await context.SaveChangesAsync();

            // 6. Seed Questions
            if (!context.Questions.Any())
            {
                var questions = new[]
                {
            new Question { Content = "Da bạn cảm thấy?", OrderNo = 1 },
            new Question { Content = "Bạn bị mụn và mụn đầu đen?", OrderNo = 2 },
            new Question { Content = "Bạn bị mụn?", OrderNo = 3 },
            new Question { Content = "Bạn thích làn da của mình khi nó?", OrderNo = 4 },
            new Question { Content = "Bạn sẽ bớt lo lắng về làn da của mình hơn nếu?", OrderNo = 5 }
        };

                await context.Questions.AddRangeAsync(questions);
                await context.SaveChangesAsync();
            }

            // 7. Seed QuestionOptions và QuestionOptionSkintypes
            if (!context.QuestionOptions.Any())
            {
                var questions = context.Questions.ToArray();
                var skinTypes = context.SkinTypes.ToArray();

                var questionOptions = new[]
                {
            new QuestionOption { Content = "A. Khô, bình tĩnh và dễ chăm sóc", QuestionID = questions[0].Id },
            new QuestionOption { Content = "B. Bóng, nhờn và có một chút vấn đề", QuestionID = questions[0].Id },
            new QuestionOption { Content = "C. Vùng trán và mũi của tôi trơn", QuestionID = questions[0].Id },
            new QuestionOption { Content = "D. Căng sau khi tôi rửa bằng chất tẩy rửa không tự nhiên", QuestionID = questions[0].Id },
            new QuestionOption { Content = "A. Luôn luôn", QuestionID = questions[1].Id },
            new QuestionOption { Content = "B. Rất hiếm khi", QuestionID = questions[1].Id },
            new QuestionOption { Content = "C. Vào thời điểm kinh nguyệt của tôi", QuestionID = questions[1].Id },
            new QuestionOption { Content = "D. Thỉnh thoảng", QuestionID = questions[1].Id },
            new QuestionOption { Content = "A. Trên trán, dọc theo đường chân tóc và trên cằm", QuestionID = questions[2].Id },
            new QuestionOption { Content = "B. Rất hiếm khi", QuestionID = questions[2].Id },
            new QuestionOption { Content = "C. Thường là khi tôi không rửa mặt bằng chất tẩy rửa tự nhiên", QuestionID = questions[2].Id },
            new QuestionOption { Content = "D. Một lần một tháng", QuestionID = questions[2].Id },
            new QuestionOption { Content = "A. Không có mụn", QuestionID = questions[3].Id },
            new QuestionOption { Content = "B. Cảm thấy sạch", QuestionID = questions[3].Id },
            new QuestionOption { Content = "C. Không đỏ và viêm", QuestionID = questions[3].Id },
            new QuestionOption { Content = "D. Trông khỏe mạnh", QuestionID = questions[3].Id },
            new QuestionOption { Content = "A. Tôi sử dụng các sản phẩm chăm sóc da tự nhiên hai lần một ngày", QuestionID = questions[4].Id },
            new QuestionOption { Content = "B. Da tôi không có cảm giác nhờn", QuestionID = questions[4].Id },
            new QuestionOption { Content = "C. Tôi yêu làn da và bản thân mình", QuestionID = questions[4].Id },
            new QuestionOption { Content = "D. Tôi không có mụn hoặc mụn đầu đen", QuestionID = questions[4].Id }
        };

                await context.QuestionOptions.AddRangeAsync(questionOptions);
                await context.SaveChangesAsync();

                var savedOptions = context.QuestionOptions.ToArray();


                // 8. Seed Bookings với 4 trạng thái và Feedbacks
                if (!context.Bookings.Any())
                {
                    var customers = context.Users.Where(u => u.Role == UserRole.Customer).ToArray();
                    var therapistIds = context.Therapists
                        .Include(t => t.UserNavigation)
                        .Where(t => t.UserNavigation != null && t.UserNavigation.Role == UserRole.Therapist)
                        .Select(t => t.Id)
                        .ToArray();
                    var services = context.Services.ToArray();

                    if (customers.Length < 3) throw new Exception("Không đủ khách hàng để seeding Bookings. Cần ít nhất 3.");
                    if (therapistIds.Length < 4) throw new Exception("Không đủ Therapist để seeding Bookings. Cần ít nhất 4.");
                    if (services.Length < 10) throw new Exception("Không đủ Services để seeding Bookings. Cần ít nhất 10.");

                    var bookings = new List<Booking>
        {
            // NotStarted
            new Booking
            {
                Id = Guid.NewGuid(),
                CustomerId = customers[0].Id,
                TherapistId = therapistIds[0],
                ServiceId = services[0].Id,
                ReservedTime = DateTime.Now.AddDays(5).AddHours(10),
                Status = BookingStatus.NotStarted,
                TotalPrice = services[0].Price,
                TotalPayment = 0 // Chưa thanh toán
            },
            // Doing
            new Booking
            {
                Id = Guid.NewGuid(),
                CustomerId = customers[1].Id,
                TherapistId = therapistIds[1],
                ServiceId = services[1].Id,
                ReservedTime = DateTime.Now.AddHours(-1), // Đang thực hiện
                Status = BookingStatus.Doing,
                TotalPrice = services[1].Price,
                TotalPayment = services[1].Price / 2 // Thanh toán một nửa
            },
            // Completed (có Feedback)
            new Booking
            {
                Id = Guid.NewGuid(),
                CustomerId = customers[2].Id,
                TherapistId = therapistIds[2],
                ServiceId = services[2].Id,
                ReservedTime = DateTime.Now.AddDays(-2).AddHours(14),
                Status = BookingStatus.Completed,
                TotalPrice = services[2].Price,
                TotalPayment = services[2].Price
            },
            // Canceled
            new Booking
            {
                Id = Guid.NewGuid(),
                CustomerId = customers[0].Id,
                TherapistId = therapistIds[3],
                ServiceId = services[3].Id,
                ReservedTime = DateTime.Now.AddDays(-1).AddHours(9),
                Status = BookingStatus.Canceled,
                TotalPrice = services[3].Price,
                TotalPayment = 0 // Không thanh toán vì hủy
            },
            // Completed thứ hai (có Feedback)
            new Booking
            {
                Id = Guid.NewGuid(),
                CustomerId = customers[1].Id,
                TherapistId = therapistIds[0],
                ServiceId = services[4].Id,
                ReservedTime = DateTime.Now.AddDays(-3).AddHours(15),
                Status = BookingStatus.Completed,
                TotalPrice = services[4].Price,
                TotalPayment = services[4].Price
            }
        };

                    await context.Bookings.AddRangeAsync(bookings);
                    await context.SaveChangesAsync();
                }

                // 9. Seed Feedbacks cho các Booking đã Completed
                if (!context.Feedbacks.Any())
                {
                    var completedBookings = context.Bookings
                        .Where(b => b.Status == BookingStatus.Completed)
                        .ToArray();

                    if (completedBookings.Length < 2) throw new Exception("Không đủ Booking Completed để seeding Feedbacks. Cần ít nhất 2.");

                    var feedbacks = new[]
                    {
            new Feedback
            {
                BookingId = completedBookings[0].Id,
                TherapistRating = 4,
                ServiceRating = 5,
                TherapistFeedback = "Nhân viên rất chuyên nghiệp và thân thiện",
                ServiceFeedback = "Dịch vụ tuyệt vời, da tôi cải thiện rõ rệt"
            },
            new Feedback
            {
                BookingId = completedBookings[1].Id,
                TherapistRating = 5,
                ServiceRating = 4,
                TherapistFeedback = "Kỹ thuật viên rất tận tâm",
                ServiceFeedback = "Dịch vụ tốt nhưng giá hơi cao"
            }
        };

                    await context.Feedbacks.AddRangeAsync(feedbacks);
                    await context.SaveChangesAsync();
                }

                // 10. Seed ServiceDetails
                if (!context.ServiceDetails.Any())
                {
                    var services = context.Services.ToArray();
                    if (services.Length < 10) throw new Exception("Không đủ Services để seeding ServiceDetails.");

                    var serviceDetails = new[]
                    {
            new ServiceDetail { Name = "Làm sạch da", Description = "Loại bỏ bụi bẩn và dầu thừa.", Duration = 30, UnitPrice = 300000, ServiceID = services[0].Id, DateToNextStep = 5, Step = 1 },
            new ServiceDetail { Name = "Điều trị mụn", Description = "Sử dụng công nghệ trị mụn.", Duration = 60, UnitPrice = 800000, ServiceID = services[1].Id, DateToNextStep = 7, Step = 1 },
            new ServiceDetail { Name = "Dưỡng trắng", Description = "Cải thiện độ sáng da.", Duration = 45, UnitPrice = 900000, ServiceID = services[2].Id, DateToNextStep = 10, Step = 1 },
            new ServiceDetail { Name = "Trẻ hóa", Description = "Kích thích collagen.", Duration = 60, UnitPrice = 1200000, ServiceID = services[3].Id, DateToNextStep = 8, Step = 1 },
            new ServiceDetail { Name = "Tẩy tế bào", Description = "Làm sạch sâu.", Duration = 20, UnitPrice = 200000, ServiceID = services[4].Id, DateToNextStep = 3, Step = 1 },
            new ServiceDetail { Name = "Massage", Description = "Thư giãn cơ mặt.", Duration = 30, UnitPrice = 350000, ServiceID = services[5].Id, DateToNextStep = 5, Step = 1 },
            new ServiceDetail { Name = "Chăm sóc sâu", Description = "Đắp mặt nạ cao cấp.", Duration = 60, UnitPrice = 1500000, ServiceID = services[6].Id, DateToNextStep = 12, Step = 1 },
            new ServiceDetail { Name = "Trị nám", Description = "Giảm sắc tố.", Duration = 45, UnitPrice = 1000000, ServiceID = services[7].Id, DateToNextStep = 7, Step = 1 },
            new ServiceDetail { Name = "Dưỡng chất", Description = "Cung cấp vitamin.", Duration = 40, UnitPrice = 500000, ServiceID = services[8].Id, DateToNextStep = 6, Step = 1 },
            new ServiceDetail { Name = "Chăm sóc mắt", Description = "Giảm quầng thâm.", Duration = 30, UnitPrice = 400000, ServiceID = services[9].Id, DateToNextStep = 4, Step = 1 }
        };

                    foreach (var detail in serviceDetails)
                    {
                        var service = services.First(s => s.Id == detail.ServiceID);
                        service.Price += detail.UnitPrice;
                    }

                    await context.ServiceDetails.AddRangeAsync(serviceDetails);
                    await context.SaveChangesAsync();
                }

                // 11. Seed Events
                if (!context.Events.Any())
                {
                    var events = new[]
                    {
        new Event
        {
            Name = "Hội thảo du lịch nghỉ dưỡng chăm sóc bản thân 2025",
            Description = "Đây là nội dung mẫu được đánh máy nhằm mục đích tạo văn bản mẫu",
            Thumbnail = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQWL6c8Zvkl4lQdlWDTBmrAUzk8WDACENRDRg&s",
            Capacity = 100,
            EventDate = DateOnly.Parse("2025/11/20"),
            Location = "Hall Alpha",
            TicketPrice = 100000,
            TimeStart = TimeOnly.Parse("12:00:00"),
            TimeEnd = TimeOnly.Parse("14:00:00")
        },
        new Event
        {
            Name = "Chuyên đề về lợi ích của hệ thống chăm sóc da",
            Description = "Đây là nội dung mẫu được đánh máy nhằm mục đích tạo văn bản mẫu",
            Capacity = 250,
            Thumbnail = "https://file.hstatic.net/200000311493/file/84_99971645a56349b8901748a211a6ca2b_grande.png",
            EventDate = DateOnly.Parse("2025/11/22"),
            Location = "Hall Alpha",
            TicketPrice = 150000,
            TimeStart = TimeOnly.Parse("07:00:00"),
            TimeEnd = TimeOnly.Parse("09:00:00")
        },
        new Event
        {
            Name = "Triển lãm công nghệ làm đẹp 2025",
            Description = "Sự kiện展示 các công nghệ làm đẹp tiên tiến nhất",
            Thumbnail = "https://nhipcauthuonghieu.vn/wp-content/uploads/2024/07/25715.jpg",
            Capacity = 150,
            EventDate = DateOnly.Parse("2025/11/25"),
            Location = "Hall Beta",
            TicketPrice = 200000,
            TimeStart = TimeOnly.Parse("09:00:00"),
            TimeEnd = TimeOnly.Parse("12:00:00")
        },
        new Event
        {
            Name = "Hội nghị sức khỏe và dinh dưỡng",
            Description = "Tìm hiểu về chế độ ăn uống lành mạnh và khoa học",
            Thumbnail = "https://i1-suckhoe.vnecdn.net/2022/07/13/young-asian-woman-holding-dumb-1785-8765-1657695811.jpg?w=1020&h=0&q=100&dpr=1&fit=crop&s=QcyeHgcnKb203eW6XGCyNQ",
            Capacity = 200,
            EventDate = DateOnly.Parse("2025/11/28"),
            Location = "Hall Gamma",
            TicketPrice = 120000,
            TimeStart = TimeOnly.Parse("14:00:00"),
            TimeEnd = TimeOnly.Parse("16:30:00")
        },
        new Event
        {
            Name = "Workshop yoga và thiền định",
            Description = "Trải nghiệm các bài tập thư giãn và cân bằng cơ thể",
            Thumbnail = "https://balanceyogavilla.com/wp-content/uploads/2024/02/balance-yoga-villa-workshop-hoi-tho-mo-rong-tam-tri-3.jpg",
            Capacity = 80,
            EventDate = DateOnly.Parse("2025/11/30"),
            Location = "Studio Delta",
            TicketPrice = 80000,
            TimeStart = TimeOnly.Parse("06:00:00"),
            TimeEnd = TimeOnly.Parse("08:00:00")
        }
    };

                    await context.Events.AddRangeAsync(events);
                    await context.SaveChangesAsync();
                }

                // 12. Seed ticket event
                if (!context.EventTickets.Any())
                {
                    var eventId = context.Events.ToArray();
                    var userId = context.Users.ToArray();
                    Console.WriteLine($"Số lượng sự kiện: {eventId.Length}");
                    //if (eventId.Length < 10) throw new Exception("Không đủ Services để seeding ServiceDetails.");

                    var eventTickets = new[]
                    {
                    new EventTicket
                    {
                        PaidAmount = 55000.00m,
                        QRCode = "QR111111",
                        TicketCode = "111111",
                        Status = EventTicketStatus.Paid,
                        UserID = userId[1].Id,
                        EventId  = eventId[1].Id,
                        CreatedTime = new DateTime(2025, 03, 21, 14, 30, 00),
                        LastUpdate = new DateTime(2025, 03, 21, 14, 35, 00)
                    },

                    new EventTicket
                    {
                        PaidAmount = 60000.50m,
                        QRCode = "QR222222",
                        TicketCode = "222222",
                        Status = EventTicketStatus.Paid,
                        UserID = userId[2].Id,
                        EventId = eventId[1].Id,
                        CreatedTime = new DateTime(2025, 03, 21, 14, 40, 00),
                        LastUpdate = new DateTime(2025, 03, 21, 14, 45, 00)
                    },

                    new EventTicket
                    {
                        PaidAmount = 75000.75m,
                        QRCode = "QR333333",
                        TicketCode = "333333",
                        Status = EventTicketStatus.Paid,
                        UserID = userId[1].Id,
                        EventId = eventId[1].Id,
                        CreatedTime = new DateTime(2025, 03, 21, 14, 50, 00),
                        LastUpdate = new DateTime(2025, 03, 21, 14, 55, 00)
                    }



                };

                    await context.EventTickets.AddRangeAsync(eventTickets);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}