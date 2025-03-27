using System;
using System.Collections.ObjectModel;
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


            // Code to use DbContext for SQL Server database engine (commented out)
            //services.AddDbContext<ApplicationDbContext>(options =>
            //{
            //    options.UseLazyLoadingProxies();
            //    options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            //});

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

                //12
                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_04",
                    FullName = "Anh Huy",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "example04@gmail.com",
                    Password = hashedPassword,
                    Phone = "0997442824",
                    DateOfBirth = DateOnly.Parse("2003/08/11"),
                    Role = UserRole.Customer
                });


                //13
                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_05",
                    FullName = "Nguyễn Huy",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "example05@gmail.com",
                    Password = hashedPassword,
                    Phone = "0997442827",
                    DateOfBirth = DateOnly.Parse("2003/08/11"),
                    Role = UserRole.Customer
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_06",
                    FullName = "Trần Minh Quân",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "minhquan06@gmail.com",
                    Password = hashedPassword,
                    Phone = "0997442828",
                    DateOfBirth = DateOnly.Parse("2001/07/15"),
                    Role = UserRole.Customer
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_07",
                    FullName = "Lê Thị Hoa",
                    Avatar = string.Empty,
                    Gender = Gender.Female,
                    Email = "lehoa07@gmail.com",
                    Password = hashedPassword,
                    Phone = "0997442829",
                    DateOfBirth = DateOnly.Parse("1999/12/05"),
                    Role = UserRole.Customer
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_08",
                    FullName = "Võ Thành Nam",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "namthanh08@gmail.com",
                    Password = hashedPassword,
                    Phone = "0997442830",
                    DateOfBirth = DateOnly.Parse("2002/06/20"),
                    Role = UserRole.Customer
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_09",
                    FullName = "Ngô Bảo Châu",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "ngochau09@gmail.com",
                    Password = hashedPassword,
                    Phone = "0997442831",
                    DateOfBirth = DateOnly.Parse("1998/11/23"),
                    Role = UserRole.Customer
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_10",
                    FullName = "Phạm Thùy Linh",
                    Avatar = string.Empty,
                    Gender = Gender.Female,
                    Email = "thuylihn10@gmail.com",
                    Password = hashedPassword,
                    Phone = "0997442832",
                    DateOfBirth = DateOnly.Parse("2000/09/17"),
                    Role = UserRole.Customer
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_11",
                    FullName = "Bùi Văn Tuấn",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "vantuant11@gmail.com",
                    Password = hashedPassword,
                    Phone = "0997442833",
                    DateOfBirth = DateOnly.Parse("2001/03/14"),
                    Role = UserRole.Customer
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_12",
                    FullName = "Đỗ Hoàng Anh",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "hoanganh12@gmail.com",
                    Password = hashedPassword,
                    Phone = "0997442834",
                    DateOfBirth = DateOnly.Parse("1997/05/09"),
                    Role = UserRole.Customer
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_13",
                    FullName = "Cao Thị Hạnh",
                    Avatar = string.Empty,
                    Gender = Gender.Female,
                    Email = "thihanh13@gmail.com",
                    Password = hashedPassword,
                    Phone = "0997442835",
                    DateOfBirth = DateOnly.Parse("2003/02/28"),
                    Role = UserRole.Customer
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_14",
                    FullName = "Lý Thành Đạt",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "thanhdat14@gmail.com",
                    Password = hashedPassword,
                    Phone = "0997442836",
                    DateOfBirth = DateOnly.Parse("1996/10/10"),
                    Role = UserRole.Customer
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_15",
                    FullName = "Trịnh Minh Khang",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "minhkhang15@gmail.com",
                    Password = hashedPassword,
                    Phone = "0997442837",
                    DateOfBirth = DateOnly.Parse("1995/08/08"),
                    Role = UserRole.Customer
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_16",
                    FullName = "Trần Hoàng Anh",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "customer16@gmail.com",
                    Password = hashedPassword,
                    Phone = "0916161616",
                    DateOfBirth = DateOnly.Parse("2000/06/16"),
                    Role = UserRole.Customer
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_17",
                    FullName = "Phạm Bảo Trâm",
                    Avatar = string.Empty,
                    Gender = Gender.Female,
                    Email = "customer17@gmail.com",
                    Password = hashedPassword,
                    Phone = "0917171717",
                    DateOfBirth = DateOnly.Parse("1999/07/17"),
                    Role = UserRole.Customer
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_18",
                    FullName = "Nguyễn Minh Đức",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "customer18@gmail.com",
                    Password = hashedPassword,
                    Phone = "0918181818",
                    DateOfBirth = DateOnly.Parse("1998/08/18"),
                    Role = UserRole.Customer
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_19",
                    FullName = "Lê Thị Thanh",
                    Avatar = string.Empty,
                    Gender = Gender.Female,
                    Email = "customer19@gmail.com",
                    Password = hashedPassword,
                    Phone = "0919191919",
                    DateOfBirth = DateOnly.Parse("1997/09/19"),
                    Role = UserRole.Customer
                });

                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer_20",
                    FullName = "Đặng Văn Sơn",
                    Avatar = string.Empty,
                    Gender = Gender.Male,
                    Email = "customer20@gmail.com",
                    Password = hashedPassword,
                    Phone = "0920202020",
                    DateOfBirth = DateOnly.Parse("1996/10/20"),
                    Role = UserRole.Customer
                });



                await context.SaveChangesAsync();
            }

            // 2. Seed ServiceCategories
            if (!context.ServiceCategories.Any())
            {
                var categories = new List<ServiceCategory>
                {
                    new ServiceCategory { Id = Guid.NewGuid(), Name = "Chăm Sóc Da Cơ Bản", Status = ServiceCategoryStatus.Enabled },
                    new ServiceCategory { Id = Guid.NewGuid(), Name = "Chăm Sóc Da Chuyên Sâu", Status = ServiceCategoryStatus.Enabled },
                    new ServiceCategory { Id = Guid.NewGuid(), Name = "Điều Trị Mụn", Status = ServiceCategoryStatus.Enabled },
                    new ServiceCategory { Id = Guid.NewGuid(), Name = "Điều Trị Nám & Tàn Nhang", Status = ServiceCategoryStatus.Enabled },
                    new ServiceCategory { Id = Guid.NewGuid(), Name = "Điều Trị Thâm & Sẹo", Status = ServiceCategoryStatus.Enabled },
                    new ServiceCategory { Id = Guid.NewGuid(), Name = "Chăm Sóc Da Nhạy Cảm", Status = ServiceCategoryStatus.Enabled },
                    new ServiceCategory { Id = Guid.NewGuid(), Name = "Chăm Sóc Da Lão Hóa", Status = ServiceCategoryStatus.Enabled },
                    new ServiceCategory { Id = Guid.NewGuid(), Name = "Trẻ Hóa Da Công Nghệ Cao", Status = ServiceCategoryStatus.Enabled },
                    new ServiceCategory { Id = Guid.NewGuid(), Name = "Peel Da & Tái Tạo Da", Status = ServiceCategoryStatus.Enabled },
                    new ServiceCategory { Id = Guid.NewGuid(), Name = "Cấp Ẩm & Phục Hồi Da", Status = ServiceCategoryStatus.Enabled },
                    new ServiceCategory { Id = Guid.NewGuid(), Name = "Cấy Tinh Chất Dưỡng Da", Status = ServiceCategoryStatus.Enabled },
                    new ServiceCategory { Id = Guid.NewGuid(), Name = "Điện Di Vitamin C", Status = ServiceCategoryStatus.Enabled },
                    new ServiceCategory { Id = Guid.NewGuid(), Name = "Thải Độc Da Công Nghệ Cao", Status = ServiceCategoryStatus.Enabled },
                    new ServiceCategory { Id = Guid.NewGuid(), Name = "Tẩy Tế Bào Chết Chuyên Sâu", Status = ServiceCategoryStatus.Enabled },
                    new ServiceCategory { Id = Guid.NewGuid(), Name = "Xông Hơi & Detox Da", Status = ServiceCategoryStatus.Enabled }

                };

                await context.ServiceCategories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            // 3. Seed SkinTypes
            if (!context.SkinTypes.Any())
            {
                var skinTypes = new[]
                {
                    new SkinType { Name = "Da khô", Description = "Da khô thường thiếu dầu và độ ẩm tự nhiên, dễ bị bong tróc và căng ráp." },
                    new SkinType { Name = "Da dầu", Description = "Da dầu có tuyến bã nhờn hoạt động mạnh, dễ bị bóng nhờn, bít tắc lỗ chân lông và nổi mụn." },
                    new SkinType { Name = "Da hỗn hợp", Description = "Da hỗn hợp có đặc điểm dầu ở vùng chữ T (trán, mũi, cằm) và khô hoặc thường ở các vùng khác." },
                    new SkinType { Name = "Da nhạy cảm", Description = "Da nhạy cảm dễ bị kích ứng, đỏ, ngứa khi tiếp xúc với các tác nhân bên ngoài như thời tiết, mỹ phẩm hoặc hóa chất." },
                    new SkinType { Name = "Da thường", Description = "Da thường có sự cân bằng giữa dầu và độ ẩm, ít gặp vấn đề về da, lỗ chân lông nhỏ và bề mặt da mịn màng." },
                    new SkinType { Name = "Da mất nước", Description = "Da mất nước có thể do thiếu nước trong lớp biểu bì, dẫn đến cảm giác khô căng và xỉn màu, ngay cả khi có dầu trên bề mặt." },
                    new SkinType { Name = "Da lão hóa", Description = "Da lão hóa có dấu hiệu nếp nhăn, chảy xệ và mất độ đàn hồi do giảm collagen và elastin theo tuổi tác." },
                    new SkinType { Name = "Da mụn", Description = "Da mụn có xu hướng dễ nổi mụn do dầu thừa, vi khuẩn và bít tắc lỗ chân lông." },
                    new SkinType { Name = "Da nhạy cảm do mụn", Description = "Da nhạy cảm bị mụn thường dễ bị kích ứng và viêm nhiễm khi tiếp xúc với hóa chất mạnh hoặc môi trường ô nhiễm." },
                    new SkinType { Name = "Da tăng sắc tố", Description = "Da bị tăng sắc tố dễ xuất hiện các vết thâm, tàn nhang hoặc nám do tác động của ánh nắng mặt trời hoặc thay đổi nội tiết tố." },
                    new SkinType { Name = "Da giãn mao mạch", Description = "Da giãn mao mạch có các mạch máu nhỏ hiện rõ trên bề mặt, thường xuất hiện ở những người có làn da mỏng hoặc nhạy cảm." },
                    new SkinType { Name = "Da dễ bị kích ứng", Description = "Da dễ bị kích ứng phản ứng mạnh với các sản phẩm chăm sóc da hoặc môi trường, gây ngứa rát hoặc đỏ da." },
                    new SkinType { Name = "Da dày sừng", Description = "Da dày sừng có lớp tế bào chết tích tụ nhiều, bề mặt sần sùi và có thể gây tắc nghẽn lỗ chân lông." },
                    new SkinType { Name = "Da bị sẹo", Description = "Da bị sẹo có vết sẹo do mụn, chấn thương hoặc phẫu thuật, có thể là sẹo lõm, sẹo lồi hoặc sẹo thâm." },
                    new SkinType { Name = "Da dễ bị viêm", Description = "Da dễ bị viêm có xu hướng phản ứng mạnh với vi khuẩn hoặc tác động bên ngoài, dẫn đến mẩn đỏ và sưng viêm." }

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
                    new Service
                    {
                        ServiceName = "Chăm sóc da mặt",
                        Description = "Dịch vụ chăm sóc da mặt chuyên sâu giúp làm sạch, cấp ẩm và tái tạo da, mang lại làn da khỏe mạnh và tươi trẻ.",
                        Price = 1200000,
                        Thumbnail = "https://thanhnien.mediacdn.vn/uploaded/quochung.qc/2018_08_28/MH1/2_RNPT.jpg?width=500",
                        Duration = 1,
                        ServiceCategoryID = serviceCategories.FirstOrDefault(c => c.Name == "Chăm Sóc Da Cơ Bản").Id,
                        ServiceImageNavigation = new Collection<ServiceImage>
                        {
                            new ServiceImage { ImageUrl = "https://tatacosmetic.vn/upload/news/1579574574-single_news23-uaruamatlamsachsauhieuqua1.jpg" },
                            new ServiceImage { ImageUrl = "https://biocyte.com.vn/wp-content/uploads/2021/07/tai-sao-phai-cap-nuoc-cho-lan-da.png" }
                        }
                    },
                    new Service
                    {
                        ServiceName = "Liệu trình Detox da",
                        Description = "Phương pháp làm sạch sâu và loại bỏ độc tố giúp làn da khỏe mạnh hơn.",
                        Price = 1500000,
                        Thumbnail = "https://skinlab.vn/wp-content/uploads/2024/05/tri-lieu-detox-thanh-loc-da.jpg",
                        Duration = 1,
                        ServiceCategoryID = serviceCategories.FirstOrDefault(c => c.Name == "Xông Hơi & Detox Da").Id,
                        ServiceImageNavigation = new Collection<ServiceImage>
                        {
                            new ServiceImage { ImageUrl = "https://anmes.vn/wp-content/uploads/2020/07/thao-duoc-cho-xong-hoi-600x343.jpg" },
                            new ServiceImage { ImageUrl = "https://bizweb.dktcdn.net/100/413/259/articles/mat-na-thai-doc.jpg?v=1676022240287" }
                        }
                    },
                    new Service
                    {
                        ServiceName = "Điều trị nám và tàn nhang",
                        Description = "Công nghệ cao giúp giảm nám, tàn nhang và làm đều màu da.",
                        Price = 2500000,
                        Thumbnail = "https://suckhoedoisong.qltns.mediacdn.vn/324455921873985536/2023/11/7/a555aade67a0b1fee8b1-16993193195541654552642.jpg",
                        Duration = 1,
                        ServiceCategoryID = serviceCategories.FirstOrDefault(c => c.Name == "Chăm Sóc Da Cơ Bản").Id,
                        ServiceImageNavigation = new Collection<ServiceImage>
                        {
                            new ServiceImage { ImageUrl = "https://cdn.nhathuoclongchau.com.vn/unsafe/800x0/filters:quality(95)/https://cms-prod.s3-sgn09.fptcloud.com/dieu_tri_nam_da_bang_laser_1_fb9019c433.jpg" },
                            new ServiceImage { ImageUrl = "https://images-1.eucerin.com/~/media/eucerin/local/vn/cham-soc-da-bang-laser/20201109_cach-lam-sach-da-mat-3.jpg?h=366&w=651&la=vi-vn" }
                        }
                    },
                    new Service
                    {
                        ServiceName = "Nâng cơ trẻ hóa da",
                        Description = "Dịch vụ sử dụng công nghệ hiện đại giúp nâng cơ và chống lão hóa.",
                        Price = 3500000,
                        Thumbnail = "https://vienthammyxuanhuong.com.vn/wp-content/uploads/2023/08/728.jpg",
                        Duration = 1,
                        ServiceCategoryID = serviceCategories.FirstOrDefault(c => c.Name == "Chăm Sóc Da Lão Hóa").Id,
                        ServiceImageNavigation = new Collection<ServiceImage>
                        {
                            new ServiceImage { ImageUrl = "https://cdn.nhathuoclongchau.com.vn/unsafe/https://cms-prod.s3-sgn09.fptcloud.com/chi_phi_lam_HIFU_0_319e19bea6.jpg" },
                            new ServiceImage { ImageUrl = "https://miri.com.vn/wp-content/uploads/2022/09/mat-na-phuc-hoi-da-la-gi-miri.com_.vn-3-1201x800.jpg" }
                        }
                    },
                    new Service
                    {
                        ServiceName = "Thải độc da bằng oxy tươi",
                        Description = "Cung cấp oxy tinh khiết giúp da khỏe mạnh và tràn đầy sức sống.",
                        Price = 2000000,
                        Thumbnail = "https://cdn.nhathuoclongchau.com.vn/unsafe/800x0/https://cms-prod.s3-sgn09.fptcloud.com/cham_soc_da_bang_oxy_tuoi_co_thuc_su_hieu_qua_1_d021ffbfde.jpg",
                        Duration = 1,
                        ServiceCategoryID = serviceCategories.FirstOrDefault(c => c.Name == "Chăm Sóc Da Chuyên Sâu").Id,
                        ServiceImageNavigation = new Collection<ServiceImage>
                        {
                            new ServiceImage { ImageUrl = "https://cdn.diemnhangroup.com/s-life/2023/03/phun-oxy-tuoi-co-tac-dung-gi-4.jpg" },
                            new ServiceImage { ImageUrl = "https://thanhhai.com.vn/wp-content/uploads/2022/07/Khi-da-duoc-cap-du-do-am.jpg" }
                        }
                    },
                    new Service
                    {
                        ServiceName = "Chăm sóc da nhạy cảm",
                        Description = "Dịch vụ đặc biệt dành cho làn da nhạy cảm, giúp phục hồi da.",
                        Price = 1600000,
                        Thumbnail = "https://suckhoedoisong.qltns.mediacdn.vn/324455921873985536/2024/11/27/da-nhay-cam-1732696074345442275689.jpg",
                        Duration = 1,
                        ServiceCategoryID = serviceCategories.FirstOrDefault(c => c.Name == "Peel Da & Tái Tạo Da").Id,
                        ServiceImageNavigation = new Collection<ServiceImage>
                        {
                            new ServiceImage { ImageUrl = "https://baamboo.com/wp-content/uploads/2021/02/lam-sach-da-mat-1.jpg" },
                            new ServiceImage { ImageUrl = "https://file.hstatic.net/1000006063/file/kem_phuc_hoi_da_giup_tai_tao_lan_da_bi_ton_thuong_7528874626b6473392ddc0cc629f0b9b_grande.jpg" }
                        }
                    },
                    new Service
                    {
                        ServiceName = "Làm sạch da sâu",
                        Description = "Quy trình 3 bước giúp loại bỏ bụi bẩn, dầu thừa, ngăn ngừa mụn.",
                        Price = 1200000,
                        Thumbnail = "https://japana.vn/uploads/detail/2021/07/images/bi-quyet-lam-sach-sau-cho-lan-da1.jpeg",
                        Duration = 1,
                        ServiceCategoryID = serviceCategories.FirstOrDefault(c => c.Name == "Tẩy Tế Bào Chết Chuyên Sâu").Id,
                        ServiceImageNavigation = new Collection<ServiceImage>
                        {
                            new ServiceImage { ImageUrl = "https://www.vimaccos.vn/public/upload/nh%20web/cach-su-dung-nuoc-tay-trang-de-co-lan-da-sach-khoe-1.jpg" },
                            new ServiceImage { ImageUrl = "https://tatacosmetic.vn/upload/detail/2020/01/images/1%20ngay%20dung%20sua%20rua%20mat%20may%20lan_2.jpg" },
                            new ServiceImage { ImageUrl = "https://bizweb.dktcdn.net/100/239/651/files/02-tay-te-bao-chet-cho-da-mun-la-house.png?v=1596705479645" }
                        }
                    },
                    new Service
                    {
                        ServiceName = "Cấp ẩm và phục hồi da",
                        Description = "2 bước giúp da mềm mại, đủ ẩm và khỏe mạnh.",
                        Price = 1500000,
                        Thumbnail = "https://www.vimed.org/wp-content/uploads/2021/06/serum-cap-am-phuc-hoi-da.jpg",
                        Duration = 1,
                        ServiceCategoryID = serviceCategories.FirstOrDefault(c => c.Name == "Cấp Ẩm & Phục Hồi Da").Id,
                        ServiceImageNavigation = new Collection<ServiceImage>
                        {
                            new ServiceImage { ImageUrl = "https://bizweb.dktcdn.net/thumb/1024x1024/100/413/259/files/mat-na-duong-am-tai-nha-15.jpg?v=1676628300013" },
                            new ServiceImage { ImageUrl = "https://megagangnam.com/wp-content/uploads/2022/12/serum-la-gi-01jpg.jpg" }
                        }
                    },
                    new Service
                    {
                        ServiceName = "Trẻ hóa da với collagen",
                        Description = "3 bước giúp tăng cường độ đàn hồi và chống lão hóa.",
                        Price = 2000000,
                        Thumbnail = "https://medicskin.vn/wp-content/uploads/2022/12/tre-hoa-da-bang-collagen.png",
                        Duration = 1,
                        ServiceCategoryID = serviceCategories.FirstOrDefault(c => c.Name == "Chăm Sóc Da Lão Hóa").Id,
                        ServiceImageNavigation = new Collection<ServiceImage>
                        {
                            new ServiceImage { ImageUrl = "https://images2.thanhnien.vn/528068263637045248/2023/5/29/image4-1685327865522406594228.png" },
                            new ServiceImage { ImageUrl = "https://dlccarevn.com/wp-content/uploads/2024/02/cf47fba7e79d36c36f8c.jpeg" },
                            new ServiceImage { ImageUrl = "https://cdn.nhathuoclongchau.com.vn/unsafe/800x0/filters:quality(95)/https://cms-prod.s3-sgn09.fptcloud.com/nhung_loai_mat_na_chong_lao_hoa_tu_thien_nhien_cho_lan_da_min_mang_tuoi_tre_1_98cc222293.jpg" }
                        }
                    },
                    new Service
                    {
                        ServiceName = "Thải độc da bằng than hoạt tính",
                        Description = "4 bước loại bỏ tạp chất, dầu thừa, giúp da sáng khỏe.",
                        Price = 1800000,
                        Thumbnail = "https://misstram.edu.vn/wp-content/uploads/2019/01/thai-doc-da-bang-than-hoat-tinh.jpg",
                        Duration = 1,
                        ServiceCategoryID = serviceCategories.FirstOrDefault(c => c.Name == "Tẩy Tế Bào Chết Chuyên Sâu").Id,
                        ServiceImageNavigation = new Collection<ServiceImage>
                        {
                            new ServiceImage { ImageUrl = "https://82xbeauty.vn/wp-content/uploads/2022/02/xong-mat-1.jpg" },
                            new ServiceImage { ImageUrl = "https://cdn.tgdd.vn//News/1486515//dap-mat-na-than-hoat-tinh-co-tac-dung-gi-top-5-1-845x500.jpg" },
                            new ServiceImage { ImageUrl = "https://aladin.com.vn/media/news/2411_serum-duong-da-min.jpg" },
                            new ServiceImage { ImageUrl = "https://medlatec.vn/media/13132/content/20201106_cac-buoc-duong-da-2.jpg" }
                        }
                    },
                    new Service
                    {
                        ServiceName = "Làm trắng da với vitamin C",
                        Description = "2 bước giúp da đều màu và rạng rỡ hơn.",
                        Price = 2500000,
                        Thumbnail = "https://s-cdn.vnluxury.vn/vnlux-media/21/6/17/Vitamin_C.jpg",
                        Duration = 1,
                        ServiceCategoryID = serviceCategories.FirstOrDefault(c => c.Name == "Điện Di Vitamin C").Id,
                        ServiceImageNavigation = new Collection<ServiceImage>
                        {
                            new ServiceImage { ImageUrl = "https://laskin.vn/wp-content/uploads/2022/09/Sau-khi-peel-da-co-nen-dung-vitamin-C-khong.jpg" },
                            new ServiceImage { ImageUrl = "https://suckhoedoisong.qltns.mediacdn.vn/324455921873985536/2024/1/2/vitamin-c-1704168408870149765090.jpeg" }
                        }
                    },
                    //// Da khô
                    //new Service { ServiceName = "Dịch vụ dưỡng ẩm sâu", Description = "Cung cấp dưỡng chất và khóa ẩm lâu dài cho da khô.", Thumbnail = "https://easysalon.vn/wp-content/uploads/2021/04/bang-gia-dich-vu-Spa-2.jpg", Duration = 1, Price = 800000, ServiceCategoryID = serviceCategories[0].Id },
                    //new Service { ServiceName = "Dịch vụ tái tạo da với mặt nạ collagen", Description = "Giảm bong tróc và tăng độ đàn hồi.", Thumbnail = "https://cdn.dealtoday.vn/img/s800x400/0341ba775a40443d85e685ef19c790ab.jpg?sign=OVhemMvb6L5yOXMFOsSjxw", Duration = 1, Price = 1200000, ServiceCategoryID = serviceCategories[0].Id },
                    //new Service { ServiceName = "Dịch vụ làm sạch nhẹ nhàng", Description = "Loại bỏ bụi bẩn mà không làm khô da.", Thumbnail = "https://cdn.dealtoday.vn/img/c280x280/LBelle-Beauty-Lay-nhan-mun-va-dien-di-lanh-phuc-hoi-avt_17092024160739.jpg?sign=cC_ZykqWSGqY2KOLsTW8IQ", Duration = 1, Price = 500000, ServiceCategoryID = serviceCategories[0].Id },
                    //new Service { ServiceName = "Dịch vụ trẻ hóa với vitamin E", Description = "Cải thiện độ căng bóng và chống oxy hóa.", Thumbnail = "https://benhvienthammynaman.com/wp-content/uploads/2023/06/truyen-vitamin-tre-hoa-da-mat-1.jpg", Duration = 1, Price = 1500000, ServiceCategoryID = serviceCategories[0].Id },

                    //// Da dầu
                    //new Service { ServiceName = "Dịch vụ kiểm soát nhờn", Description = "Giảm tiết dầu và se khít lỗ chân lông.", Thumbnail = "https://o2skin.vn/wp-content/uploads/2024/05/hinh-anh-gioi-thieu-dich-vu-mat-na-dieu-tri-mun-va-kiem-soat-nhon-3.png", Duration = 1, Price = 900000, ServiceCategoryID = serviceCategories[1].Id },
                    //new Service { ServiceName = "Dịch vụ trị mụn chuyên sâu", Description = "Giảm viêm và ngăn ngừa mụn tái phát.", Thumbnail = "https://anabelspa.vn/wp-content/uploads/2023/06/dieu-tri-mun-chuyen-sau-500x467.jpg", Duration = 1, Price = 1200000, ServiceCategoryID = serviceCategories[1].Id },
                    //new Service { ServiceName = "Dịch vụ tẩy tế bào chết hóa học", Description = "Loại bỏ tế bào chết và kiểm soát dầu thừa.", Thumbnail = "https://www.elle.vn/app/uploads/2020/08/13/411653/da-dau-1.jpg", Duration = 1, Price = 800000, ServiceCategoryID = serviceCategories[1].Id },
                    //new Service { ServiceName = "Dịch vụ làm sạch sâu với than hoạt tính", Description = "Hút nhờn và giảm mụn đầu đen.", Thumbnail = "https://haianhspa.com.vn/wp-content/uploads/2020/03/IMG_8937-1536x1024.jpg", Duration = 1, Price = 1000000, ServiceCategoryID = serviceCategories[1].Id },

                    //// Da hỗn hợp
                    //new Service { ServiceName = "Dịch vụ cân bằng độ ẩm vùng chữ T", Description = "Giảm nhờn vùng trán và dưỡng ẩm vùng má.", Thumbnail = "https://bloganchoi.com/wp-content/uploads/2016/09/vung-chu-t-la-gi.jpg", Duration = 1, Price = 1000000, ServiceCategoryID = serviceCategories[2].Id },
                    //new Service { ServiceName = "Dịch vụ trị mụn cục bộ", Description = "Đặc trị mụn ở vùng trán và cằm.", Thumbnail = "https://blissbeauty.vn/wp-content/uploads/2023/03/7.png", Duration = 1, Price = 1200000, ServiceCategoryID = serviceCategories[2].Id },
                    //new Service { ServiceName = "Dịch vụ làm sạch 2 bước", Description = "Làm sạch dầu vùng chữ T và giữ ẩm vùng má.", Thumbnail = "https://linhtranspa.com/wp-content/uploads/2021/05/dich-vu-cham-soc-da-mat-chuyen-sau-2.jpg", Duration = 1, Price = 800000, ServiceCategoryID = serviceCategories[2].Id },
                    //new Service { ServiceName = "Dịch vụ tẩy da chết enzyme", Description = "Loại bỏ tế bào chết nhẹ nhàng và không gây khô da.", Thumbnail = "https://thammymisstram.vn/wp-content/uploads/2021/08/tay-da-chet-bang-enzyme.jpg", Duration = 1, Price = 900000, ServiceCategoryID = serviceCategories[2].Id },
                    //new Service { ServiceName = "Dịch vụ trẻ hóa với vitamin E", Description = "Cải thiện độ căng bóng và chống oxy hóa.", Thumbnail = "https://https://5.imimg.com/data5/SELLER/Default/2021/3/GU/TE/RL/31552095/ladies-facial-services-1000x1000.jpg", Duration = 60, Price = 1500000, ServiceCategoryID = serviceCategories[0].Id, ServiceDetailNavigation = new List<ServiceDetail> { new ServiceDetail { Step = 1, Name = "Làm sạch da", Duration = 15, Description = "Làm sạch sâu giúp loại bỏ bụi bẩn và dầu thừa.", DateToNextStep = 3 }, new ServiceDetail { Step = 2, Name = "Dưỡng da với vitamin E", Duration = 30, Description = "Cung cấp dưỡng chất giúp da căng bóng và đàn hồi.", DateToNextStep = 3 }, new ServiceDetail { Step = 3, Name = "Khóa ẩm", Duration = 15, Description = "Tăng cường độ ẩm giúp da mềm mại và tươi trẻ.", DateToNextStep = 0} } }

                };


                await context.Services.AddRangeAsync(services);
                await context.SaveChangesAsync();
            }

            // 4.2. Seed ServiceDetails
            if (!context.ServiceDetails.Any())
            {
                var services = context.Services.ToArray();
                

                var serviceDetails = new[]
                {
                     new ServiceDetail { Name = "Làm sạch sâu", Description = "Bước làm sạch sâu giúp loại bỏ bụi bẩn, bã nhờn và lớp trang điểm trên da, giúp da thông thoáng và hấp thụ dưỡng chất tốt hơn.", Duration = 30, UnitPrice = 400000, ServiceID = services[0].Id, DateToNextStep = 7, Step = 1 },
                    new ServiceDetail { Name = "Cấp ẩm", Description = "Cung cấp độ ẩm chuyên sâu cho làn da bằng các tinh chất dưỡng ẩm cao cấp, giúp da căng mọng và mịn màng hơn.", Duration = 40, UnitPrice = 800000, ServiceID = services[0].Id, DateToNextStep = 0, Step = 2 },
                    new ServiceDetail { Name = "Xông hơi thảo dược", Description = "Giúp mở lỗ chân lông và đào thải độc tố trong da.", Duration = 30, UnitPrice = 500000, ServiceID = services[1].Id, DateToNextStep = 5, Step = 1 },
                    new ServiceDetail { Name = "Đắp mặt nạ thải độc", Description = "Mặt nạ than hoạt tính giúp hấp thụ dầu thừa và làm sạch sâu lỗ chân lông.", Duration = 40, UnitPrice = 1000000, ServiceID = services[1].Id, DateToNextStep = 0, Step = 2 },
                    new ServiceDetail { Name = "Laser trị nám", Description = "Sử dụng tia laser phá vỡ sắc tố nám trên da.", Duration = 60, UnitPrice = 1500000, ServiceID = services[2].Id, DateToNextStep = 2, Step = 1 },
                    new ServiceDetail { Name = "Dưỡng trắng sau laser", Description = "Dưỡng da bằng serum làm dịu da và tăng cường độ sáng.", Duration = 40, UnitPrice = 1000000, ServiceID = services[2].Id, DateToNextStep = 0, Step = 2 },
                    new ServiceDetail { Name = "Công nghệ HIFU", Description = "Sóng siêu âm hội tụ giúp nâng cơ mặt hiệu quả.", Duration = 60, UnitPrice = 2000000, ServiceID = services[3].Id, DateToNextStep = 3, Step = 1 },
                    new ServiceDetail { Name = "Mặt nạ phục hồi", Description = "Làm dịu da và kích thích sản sinh collagen.", Duration = 40, UnitPrice = 1500000, ServiceID = services[3].Id, DateToNextStep = 0, Step = 2 },
                    new ServiceDetail { Name = "Xịt oxy tươi", Description = "Làm sạch và cung cấp oxy sâu vào da.", Duration = 30, UnitPrice = 1000000, ServiceID = services[4].Id, DateToNextStep = 7, Step = 1 },
                    new ServiceDetail { Name = "Dưỡng ẩm cấp tốc", Description = "Sử dụng serum cấp ẩm giúp da căng bóng ngay lập tức.", Duration = 40, UnitPrice = 1000000, ServiceID = services[4].Id, DateToNextStep = 0, Step = 2 },
                    new ServiceDetail { Name = "Làm sạch nhẹ nhàng", Description = "Sử dụng sữa rửa mặt dịu nhẹ phù hợp với da nhạy cảm.", Duration = 30, UnitPrice = 800000, ServiceID = services[5].Id, DateToNextStep = 4, Step = 1 },
                    new ServiceDetail { Name = "Dưỡng da phục hồi", Description = "Serum chứa thành phần phục hồi giúp da khỏe mạnh hơn.", Duration = 40, UnitPrice = 800000, ServiceID = services[5].Id, DateToNextStep = 0, Step = 2 },
                    new ServiceDetail { Name = "Tẩy trang dịu nhẹ", Description = "Loại bỏ lớp trang điểm mà không gây khô da.", Duration = 15, UnitPrice = 300000, ServiceID = services[6].Id, DateToNextStep = 2, Step = 1 },
                    new ServiceDetail { Name = "Sữa rửa mặt chuyên dụng", Description = "Làm sạch sâu và cung cấp độ ẩm cho da.", Duration = 20, UnitPrice = 400000, ServiceID = services[6].Id, DateToNextStep = 3, Step = 2 },
                    new ServiceDetail { Name = "Tẩy tế bào chết nhẹ nhàng", Description = "Loại bỏ da chết, giúp hấp thụ dưỡng chất tốt hơn.", Duration = 15, UnitPrice = 500000, ServiceID = services[6].Id, DateToNextStep = 0, Step = 3 },
                    new ServiceDetail { Name = "Đắp mặt nạ dưỡng ẩm", Description = "Cung cấp nước và làm dịu da khô, nhạy cảm.", Duration = 30, UnitPrice = 700000, ServiceID = services[7].Id, DateToNextStep = 4, Step = 1 },
                    new ServiceDetail { Name = "Tinh chất dưỡng sâu", Description = "Serum cấp ẩm giúp da căng bóng tự nhiên.", Duration = 20, UnitPrice = 800000, ServiceID = services[7].Id, DateToNextStep = 0, Step = 2 },
                    new ServiceDetail { Name = "Bổ sung tinh chất collagen", Description = "Làm đầy rãnh nhăn và kích thích tái tạo tế bào.", Duration = 40, UnitPrice = 800000, ServiceID = services[8].Id, DateToNextStep = 5, Step = 1 },
                    new ServiceDetail { Name = "Massage nâng cơ", Description = "Giúp săn chắc cơ mặt, giảm chảy xệ.", Duration = 30, UnitPrice = 600000, ServiceID = services[8].Id, DateToNextStep = 3, Step = 2 },
                    new ServiceDetail { Name = "Mặt nạ chống lão hóa", Description = "Cung cấp dưỡng chất giúp da trẻ hóa.", Duration = 30, UnitPrice = 600000, ServiceID = services[8].Id, DateToNextStep = 0, Step = 3 },
                    new ServiceDetail { Name = "Xông hơi thảo dược", Description = "Mở lỗ chân lông, hỗ trợ đào thải độc tố.", Step = 1, Duration = 20, UnitPrice = 400000, ServiceID = services[9].Id, DateToNextStep = 2 },
                    new ServiceDetail { Name = "Mặt nạ than hoạt tính", Description = "Hấp thụ bã nhờn và giảm nguy cơ mụn đầu đen.", Step = 2, Duration = 30, UnitPrice = 500000, ServiceID = services[9].Id, DateToNextStep = 2 },
                    new ServiceDetail { Name = "Serum thải độc da", Description = "Cung cấp dưỡng chất giúp làm sạch sâu từ bên trong.", Step = 3, Duration = 30, UnitPrice = 500000, ServiceID = services[9].Id, DateToNextStep = 1 },
                    new ServiceDetail { Name = "Dưỡng ẩm kết thúc quy trình", Description = "Giữ da mềm mịn và cân bằng độ pH.", Step = 4, Duration = 20, UnitPrice = 400000, ServiceID = services[9].Id, DateToNextStep = 0 },
                    new ServiceDetail { Name = "Peel da vitamin C", Description = "Loại bỏ lớp tế bào chết sạm màu, tái tạo da.", Step = 1, Duration = 30, UnitPrice = 1200000, ServiceID = services[10].Id, DateToNextStep = 5 },
                    new ServiceDetail { Name = "Serum vitamin C", Description = "Cải thiện độ sáng da và chống oxy hóa.", Step = 2, Duration = 40, UnitPrice = 1300000, ServiceID = services[10].Id, DateToNextStep = 0 }
                };

                foreach (var detail in serviceDetails)
                {
                    var service = services.First(s => s.Id == detail.ServiceID);
                    service.Price += detail.UnitPrice;
                }

                await context.ServiceDetails.AddRangeAsync(serviceDetails);
                await context.SaveChangesAsync();
            }

            // 5. Seed ServiceRecommendations
        //    if (!context.ServiceRecommendation.Any())
        //    {
        //        var skinTypes = context.SkinTypes.ToArray();
        //        var services = context.Services.ToArray();

        //        if (skinTypes.Length < 3)
        //        {
        //            throw new Exception("Không đủ SkinTypes để seeding ServiceRecommendations. Cần ít nhất 3 loại da.");
        //        }
        //        if (services.Length < 10)
        //        {
        //            throw new Exception("Không đủ Services để seeding ServiceRecommendations. Cần ít nhất 10 dịch vụ.");
        //        }

        //        var recommendations = new[]
        //        {
        //    new ServiceRecommendation { SkinTypeID = skinTypes[0].Id, ServiceID = services[0].Id },
        //    new ServiceRecommendation { SkinTypeID = skinTypes[1].Id, ServiceID = services[1].Id },
        //    new ServiceRecommendation { SkinTypeID = skinTypes[2].Id, ServiceID = services[2].Id },
        //    new ServiceRecommendation { SkinTypeID = skinTypes[0].Id, ServiceID = services[3].Id },
        //    new ServiceRecommendation { SkinTypeID = skinTypes[1].Id, ServiceID = services[4].Id },
        //    new ServiceRecommendation { SkinTypeID = skinTypes[2].Id, ServiceID = services[5].Id },
        //    new ServiceRecommendation { SkinTypeID = skinTypes[0].Id, ServiceID = services[6].Id },
        //    new ServiceRecommendation { SkinTypeID = skinTypes[1].Id, ServiceID = services[7].Id },
        //    new ServiceRecommendation { SkinTypeID = skinTypes[2].Id, ServiceID = services[8].Id },
        //    new ServiceRecommendation { SkinTypeID = skinTypes[0].Id, ServiceID = services[9].Id }
        //};

        //        await context.ServiceRecommendation.AddRangeAsync(recommendations);
        //        await context.SaveChangesAsync();
        //    }

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
//                var optionSkinTypes = new[] {
//    // Question 0 options (index 0-3)
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[0].Id, SkinTypeID = skinTypes[0].Id }, // A. Khô -> Da khô
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[0].Id, SkinTypeID = skinTypes[4].Id }, // A. Dễ chăm sóc -> Da thường
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[1].Id, SkinTypeID = skinTypes[1].Id }, // B. Bóng, nhờn -> Da dầu
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[1].Id, SkinTypeID = skinTypes[2].Id }, // B. Nhờn -> Da hỗn hợp
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[2].Id, SkinTypeID = skinTypes[2].Id }, // C. Trán, mũi trơn -> Da hỗn hợp
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[2].Id, SkinTypeID = skinTypes[1].Id }, // C. Trơn -> Da dầu
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[3].Id, SkinTypeID = skinTypes[3].Id }, // D. Căng sau rửa -> Da nhạy cảm
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[3].Id, SkinTypeID = skinTypes[0].Id }, // D. Căng -> Da khô

//    // Question 1 options (index 4-7)
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[4].Id, SkinTypeID = skinTypes[1].Id }, // A. Luôn luôn (mụn) -> Da dầu
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[4].Id, SkinTypeID = skinTypes[2].Id }, // A. Luôn luôn -> Da hỗn hợp
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[5].Id, SkinTypeID = skinTypes[4].Id }, // B. Rất hiếm khi -> Da thường
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[5].Id, SkinTypeID = skinTypes[0].Id }, // B. Hiếm mụn -> Da khô
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[6].Id, SkinTypeID = skinTypes[2].Id }, // C. Khi kinh nguyệt -> Da hỗn hợp
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[6].Id, SkinTypeID = skinTypes[1].Id }, // C. Khi kinh nguyệt -> Da dầu
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[7].Id, SkinTypeID = skinTypes[3].Id }, // D. Thỉnh thoảng -> Da nhạy cảm
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[7].Id, SkinTypeID = skinTypes[4].Id }, // D. Thỉnh thoảng -> Da thường

//    // Question 2 options (index 8-11)
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[8].Id, SkinTypeID = skinTypes[2].Id }, // A. Trán, cằm -> Da hỗn hợp
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[8].Id, SkinTypeID = skinTypes[1].Id }, // A. Trán, cằm -> Da dầu
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[9].Id, SkinTypeID = skinTypes[4].Id }, // B. Rất hiếm khi -> Da thường
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[9].Id, SkinTypeID = skinTypes[0].Id }, // B. Hiếm khi -> Da khô
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[10].Id, SkinTypeID = skinTypes[3].Id }, // C. Không rửa tự nhiên -> Da nhạy cảm
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[10].Id, SkinTypeID = skinTypes[0].Id }, // C. Không rửa -> Da khô
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[11].Id, SkinTypeID = skinTypes[1].Id }, // D. Một lần/tháng -> Da dầu
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[11].Id, SkinTypeID = skinTypes[2].Id }, // D. Một lần/tháng -> Da hỗn hợp

//    // Question 3 options (index 12-15)
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[12].Id, SkinTypeID = skinTypes[4].Id }, // A. Không mụn -> Da thường
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[12].Id, SkinTypeID = skinTypes[0].Id }, // A. Không mụn -> Da khô
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[13].Id, SkinTypeID = skinTypes[4].Id }, // B. Cảm thấy sạch -> Da thường
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[13].Id, SkinTypeID = skinTypes[0].Id }, // B. Sạch -> Da khô
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[14].Id, SkinTypeID = skinTypes[3].Id }, // C. Không đỏ, viêm -> Da nhạy cảm
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[14].Id, SkinTypeID = skinTypes[4].Id }, // C. Không viêm -> Da thường
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[15].Id, SkinTypeID = skinTypes[4].Id }, // D. Trông khỏe mạnh -> Da thường
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[15].Id, SkinTypeID = skinTypes[0].Id }, // D. Khỏe mạnh -> Da khô

//    // Question 4 options (index 16-19)
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[16].Id, SkinTypeID = skinTypes[4].Id }, // A. Chăm sóc tự nhiên -> Da thường
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[16].Id, SkinTypeID = skinTypes[3].Id }, // A. Chăm sóc -> Da nhạy cảm
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[17].Id, SkinTypeID = skinTypes[0].Id }, // B. Không nhờn -> Da khô
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[17].Id, SkinTypeID = skinTypes[4].Id }, // B. Không nhờn -> Da thường
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[18].Id, SkinTypeID = skinTypes[4].Id }, // C. Yêu làn da -> Da thường
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[18].Id, SkinTypeID = skinTypes[0].Id }, // C. Yêu làn da -> Da khô
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[19].Id, SkinTypeID = skinTypes[4].Id }, // D. Không mụn -> Da thường
//    new QuestionOptionSkintype { QuestionOptionId = savedOptions[19].Id, SkinTypeID = skinTypes[0].Id }  // D. Không mụn -> Da khô
//};

//                await context.QuestionOptionSkintypes.AddRangeAsync(optionSkinTypes);
                await context.SaveChangesAsync();
            }

        //    // 8. Seed Bookings với 4 trạng thái và Feedbacks
        //    if (!context.Bookings.Any())
        //    {
        //        var customers = context.Users.Where(u => u.Role == UserRole.Customer).ToArray();
        //        var therapistIds = context.Therapists
        //            .Include(t => t.UserNavigation)
        //            .Where(t => t.UserNavigation != null && t.UserNavigation.Role == UserRole.Therapist)
        //            .Select(t => t.Id)
        //            .ToArray();
        //        var services = context.Services.ToArray();

        //        if (customers.Length < 3) throw new Exception("Không đủ khách hàng để seeding Bookings. Cần ít nhất 3.");
        //        if (therapistIds.Length < 4) throw new Exception("Không đủ Therapist để seeding Bookings. Cần ít nhất 4.");
        //        if (services.Length < 10) throw new Exception("Không đủ Services để seeding Bookings. Cần ít nhất 10.");

        //        var bookings = new List<Booking>
        //{
        //    // NotStarted
        //    new Booking
        //    {
        //        Id = Guid.NewGuid(),
        //        CustomerId = customers[0].Id,
        //        TherapistId = therapistIds[0],
        //        ServiceId = services[0].Id,
        //        ReservedTime = DateTime.Now.AddDays(5).AddHours(10),
        //        Status = BookingStatus.NotStarted,
        //        TotalPrice = services[0].Price,
        //        TotalPayment = 0 // Chưa thanh toán
        //    },
        //    // Doing
        //    new Booking
        //    {
        //        Id = Guid.NewGuid(),
        //        CustomerId = customers[1].Id,
        //        TherapistId = therapistIds[1],
        //        ServiceId = services[1].Id,
        //        ReservedTime = DateTime.Now.AddHours(-1), // Đang thực hiện
        //        Status = BookingStatus.Doing,
        //        TotalPrice = services[1].Price,
        //        TotalPayment = services[1].Price / 2 // Thanh toán một nửa
        //    },
        //    // Completed (có Feedback)
        //    new Booking
        //    {
        //        Id = Guid.NewGuid(),
        //        CustomerId = customers[2].Id,
        //        TherapistId = therapistIds[2],
        //        ServiceId = services[2].Id,
        //        ReservedTime = DateTime.Now.AddDays(-2).AddHours(14),
        //        Status = BookingStatus.Completed,
        //        TotalPrice = services[2].Price,
        //        TotalPayment = services[2].Price
        //    },
        //    // Canceled
        //    new Booking
        //    {
        //        Id = Guid.NewGuid(),
        //        CustomerId = customers[0].Id,
        //        TherapistId = therapistIds[3],
        //        ServiceId = services[3].Id,
        //        ReservedTime = DateTime.Now.AddDays(-1).AddHours(9),
        //        Status = BookingStatus.Canceled,
        //        TotalPrice = services[3].Price,
        //        TotalPayment = 0 // Không thanh toán vì hủy
        //    },
        //    // Completed thứ hai (có Feedback)
        //    new Booking
        //    {
        //        Id = Guid.NewGuid(),
        //        CustomerId = customers[1].Id,
        //        TherapistId = therapistIds[0],
        //        ServiceId = services[4].Id,
        //        ReservedTime = DateTime.Now.AddDays(-3).AddHours(15),
        //        Status = BookingStatus.Completed,
        //        TotalPrice = services[4].Price,
        //        TotalPayment = services[4].Price
        //    }
        //};

        //        await context.Bookings.AddRangeAsync(bookings);
        //        await context.SaveChangesAsync();
        //    }

        //    // 9. Seed Feedbacks cho các Booking đã Completed
        //    if (!context.Feedbacks.Any())
        //    {
        //        var completedBookings = context.Bookings
        //            .Where(b => b.Status == BookingStatus.Completed)
        //            .ToArray();

        //        if (completedBookings.Length < 2) throw new Exception("Không đủ Booking Completed để seeding Feedbacks. Cần ít nhất 2.");

        //        var feedbacks = new[]
        //        {
        //    new Feedback
        //    {
        //        BookingId = completedBookings[0].Id,
        //        TherapistRating = 4,
        //        ServiceRating = 5,
        //        TherapistFeedback = "Nhân viên rất chuyên nghiệp và thân thiện",
        //        ServiceFeedback = "Dịch vụ tuyệt vời, da tôi cải thiện rõ rệt"
        //    },
        //    new Feedback
        //    {
        //        BookingId = completedBookings[1].Id,
        //        TherapistRating = 5,
        //        ServiceRating = 4,
        //        TherapistFeedback = "Kỹ thuật viên rất tận tâm",
        //        ServiceFeedback = "Dịch vụ tốt nhưng giá hơi cao"
        //    }
        //};

        //        await context.Feedbacks.AddRangeAsync(feedbacks);
        //        await context.SaveChangesAsync();
        //    }

            

            // 11. Seed Events
            if (!context.Events.Any())
            {
                var events = new[]
                {
                    new Event
                    {
                        Name = "Skincare Workshop",
                        Capacity = 50,
                        TicketPrice = 150000,
                        Description = "Hội thảo về chăm sóc da với chuyên gia",
                        EventDate = DateOnly.Parse("2025-04-10"),
                        TimeStart = TimeOnly.Parse("14:00:00"),
                        TimeEnd = TimeOnly.Parse("17:00:00"),
                        Location = "Hà Nội",
                        Thumbnail = "https://www.gotmink.pt/wp-content/uploads/2020/11/capa-930x620.jpg",
                        Status = EventStatus.Approved
                    },
                    new Event
                    {
                        Name = "Anti-aging Techniques",
                        Capacity = 40,
                        TicketPrice = 200000,
                        Description = "Bí quyết chống lão hóa cho làn da tươi trẻ",
                        EventDate = DateOnly.Parse("2025-05-12"),
                        TimeStart = TimeOnly.Parse("10:00:00"),
                        TimeEnd = TimeOnly.Parse("13:00:00"),
                        Location = "TP. HCM",
                        Thumbnail = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSEjJbv-NQ8ASDd8fxFyq6yWWKAdF2Xf7dpFA&s",
                        Status = EventStatus.Approved
                    },
                    new Event
                    {
                        Name = "Natural Skincare",
                        Capacity = 60,
                        TicketPrice = 180000,
                        Description = "Chăm sóc da bằng nguyên liệu thiên nhiên",
                        EventDate = DateOnly.Parse("2025-06-15"),
                        TimeStart = TimeOnly.Parse("09:00:00"),
                        TimeEnd = TimeOnly.Parse("12:00:00"),
                        Location = "Đà Nẵng",
                        Thumbnail = "https://d1csarkz8obe9u.cloudfront.net/posterpreviews/skin-care-flyers-design-template-57ce05f7cff5d8db4675a1034a4d9712_screen.jpg?ts=1621586739",
                        Status = EventStatus.Approved
                    },
                    new Event
                    {
                        Name = "Sun Protection Seminar",
                        Capacity = 30,
                        TicketPrice = 120000,
                        Description = "Cách bảo vệ da khỏi tác hại của ánh nắng mặt trời",
                        EventDate = DateOnly.Parse("2025-07-20"),
                        TimeStart = TimeOnly.Parse("15:00:00"),
                        TimeEnd = TimeOnly.Parse("17:30:00"),
                        Location = "Hà Nội",
                        Thumbnail = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS_oCNbN6xQUVb1NzR9WSXZvWnAAVjsZjWUag&s",
                        Status = EventStatus.Approved
                    },
                    new Event
                    {
                        Name = "Acne Treatment Session",
                        Capacity = 25,
                        TicketPrice = 100000,
                        Description = "Hướng dẫn trị mụn hiệu quả và ngăn ngừa tái phát",
                        EventDate = DateOnly.Parse("2025-08-05"),
                        TimeStart = TimeOnly.Parse("16:00:00"),
                        TimeEnd = TimeOnly.Parse("18:30:00"),
                        Location = "TP. HCM",
                        Thumbnail = "https://www.laroche-posay.us/dw/image/v2/AANG_PRD/on/demandware.static/-/Sites-lrp-us-Library/default/dw02903768/pages/Acne%20Positivity%20LP/AcnePos_CP_CRSL_Banner2_MOB_640x650.jpg?sw=640&sh=650&sm=cut&q=70",
                        Status = EventStatus.ApprovePending
                    },
                    new Event
                    {
                        Name = "Night Skincare Routine",
                        Capacity = 35,
                        TicketPrice = 170000,
                        Description = "Quy trình chăm sóc da buổi tối cho làn da khỏe mạnh",
                        EventDate = DateOnly.Parse("2025-09-10"),
                        TimeStart = TimeOnly.Parse("18:00:00"),
                        TimeEnd = TimeOnly.Parse("21:00:00"),
                        Location = "Hải Phòng",
                        Thumbnail = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSYpce7c_1Gqrk-KATJOc0SnO0mKmxI-xQqksUBZgseSvgsF9-3tCVY9qlOYTEVi7UTWsI&usqp=CAU",
                        Status = EventStatus.Approved
                    },
                    new Event
                    {
                        Name = "Hydration & Moisturizing",
                        Capacity = 50,
                        TicketPrice = 160000,
                        Description = "Dưỡng ẩm đúng cách cho từng loại da",
                        EventDate = DateOnly.Parse("2025-10-05"),
                        TimeStart = TimeOnly.Parse("11:00:00"),
                        TimeEnd = TimeOnly.Parse("14:00:00"),
                        Location = "Cần Thơ",
                        Thumbnail = "https://cdn.shopify.com/s/files/1/0508/8860/5891/files/MODULO_10_10.png?v=1729625168",
                        Status = EventStatus.Approved
                    },
                    new Event
                    {
                        Name = "Skincare for Men",
                        Capacity = 45,
                        TicketPrice = 140000,
                        Description = "Hướng dẫn chăm sóc da dành riêng cho nam giới",
                        EventDate = DateOnly.Parse("2025-11-12"),
                        TimeStart = TimeOnly.Parse("13:00:00"),
                        TimeEnd = TimeOnly.Parse("16:00:00"),
                        Location = "Hà Nội",
                        Thumbnail = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTwAxwStJIZmygTFzDs5iF6HaHGJh9Utj575A&s",
                        Status = EventStatus.Approved
                    },
                    new Event
                    {
                        Name = "Seasonal Skincare Tips",
                        Capacity = 55,
                        TicketPrice = 190000,
                        Description = "Cách chăm sóc da theo từng mùa",
                        EventDate = DateOnly.Parse("2025-12-20"),
                        TimeStart = TimeOnly.Parse("09:00:00"),
                        TimeEnd = TimeOnly.Parse("11:30:00"),
                        Location = "TP. HCM",
                        Thumbnail = "https://www.botanicalscience.net/wp-content/uploads/2024/08/Seasonal-Skincare-Feature-Image_240805.jpg",
                        Status = EventStatus.ApprovePending
                    },
                    new Event
                    {
                        Name = "Skincare & Makeup",
                        Capacity = 70,
                        TicketPrice = 220000,
                        Description = "Kết hợp chăm sóc da và trang điểm tự nhiên",
                        EventDate = DateOnly.Parse("2026-01-15"),
                        TimeStart = TimeOnly.Parse("10:30:00"),
                        TimeEnd = TimeOnly.Parse("14:30:00"),
                        Location = "Đà Nẵng",
                        Thumbnail = "https://i.ytimg.com/vi/kPOSNVrSadI/maxresdefault.jpg",
                        Status = EventStatus.Approved
                    }
       
                };

                await context.Events.AddRangeAsync(events);
                await context.SaveChangesAsync();
            }

            //// 12. Seed ticket event
            //if (!context.EventTickets.Any())
            //{
            //    var eventId = context.Events.ToArray();
            //    var userId = context.Users.ToArray();
            //    Console.WriteLine($"Số lượng sự kiện: {eventId.Length}");
            //    //if (eventId.Length < 10) throw new Exception("Không đủ Services để seeding ServiceDetails.");

            //    var eventTickets  = new[]
            //    {
            //        new EventTicket { EventId = eventId[1].Id, UserID = userId[9].Id, PaidAmount = 150000m, QRCode = "QR123ABC", TicketCode = "445278", Status = EventTicketStatus.Paid },
            //        new EventTicket { EventId = eventId[1].Id, UserID = userId[10].Id, PaidAmount = 150000m, QRCode = "QR124ABC", TicketCode = "582758", Status = EventTicketStatus.Paid },
            //        new EventTicket { EventId = eventId[2].Id, UserID = userId[11].Id, PaidAmount = 200000m, QRCode = "QR125ABC", TicketCode = "698550", Status = EventTicketStatus.Paid },
            //        new EventTicket { EventId = eventId[2].Id, UserID = userId[12].Id, PaidAmount = 200000m, QRCode = "QR126ABC", TicketCode = "702343", Status = EventTicketStatus.Paid },
            //        new EventTicket { EventId = eventId[3].Id, UserID = userId[13].Id, PaidAmount = 180000m, QRCode = "QR127ABC", TicketCode = "489408", Status = EventTicketStatus.Paid },
            //        new EventTicket { EventId = eventId[3].Id, UserID = userId[14].Id, PaidAmount = 180000m, QRCode = "QR128ABC", TicketCode = "418738", Status = EventTicketStatus.Paid },
            //        new EventTicket { EventId = eventId[4].Id, UserID = userId[15].Id, PaidAmount = 120000m, QRCode = "QR129ABC", TicketCode = "357431", Status = EventTicketStatus.Paid },
            //        new EventTicket { EventId = eventId[4].Id, UserID = userId[16].Id, PaidAmount = 120000m, QRCode = "QR130ABC", TicketCode = "300741", Status = EventTicketStatus.Paid },
            //        new EventTicket { EventId = eventId[6].Id, UserID = userId[17].Id, PaidAmount = 170000m, QRCode = "QR131ABC", TicketCode = "749075", Status = EventTicketStatus.Paid },
            //        new EventTicket { EventId = eventId[6].Id, UserID = userId[18].Id, PaidAmount = 170000m, QRCode = "QR132ABC", TicketCode = "259130", Status = EventTicketStatus.Paid },
            //        new EventTicket { EventId = eventId[6].Id, UserID = userId[19].Id, PaidAmount = 170000m, QRCode = "QR133ABC", TicketCode = "549310", Status = EventTicketStatus.Paid },
            //        new EventTicket { EventId = eventId[8].Id, UserID = userId[20].Id, PaidAmount = 140000m, QRCode = "QR134ABC", TicketCode = "533272", Status = EventTicketStatus.Paid },
            //        new EventTicket { EventId = eventId[8].Id, UserID = userId[21].Id, PaidAmount = 140000m, QRCode = "QR135ABC", TicketCode = "574362", Status = EventTicketStatus.Paid },
            //        new EventTicket { EventId = eventId[8].Id, UserID = userId[22].Id, PaidAmount = 140000m, QRCode = "QR136ABC", TicketCode = "277445", Status = EventTicketStatus.Paid },
            //        new EventTicket { EventId = eventId[10].Id, UserID = userId[23].Id, PaidAmount = 220000m, QRCode = "QR137ABC", TicketCode = "659561", Status = EventTicketStatus.Paid },
            //        new EventTicket { EventId = eventId[10].Id, UserID = userId[24].Id, PaidAmount = 220000m, QRCode = "QR138ABC", TicketCode = "308473", Status = EventTicketStatus.Paid },
            //        new EventTicket { EventId = eventId[10].Id, UserID = userId[25].Id, PaidAmount = 220000m, QRCode = "QR139ABC", TicketCode = "996688", Status = EventTicketStatus.Paid },
            //        new EventTicket { EventId = eventId[10].Id, UserID = userId[26].Id, PaidAmount = 220000m, QRCode = "QR140ABC", TicketCode = "719042", Status = EventTicketStatus.Paid },
            //        new EventTicket { EventId = eventId[10].Id, UserID = userId[27].Id, PaidAmount = 220000m, QRCode = "QR141ABC", TicketCode = "535380", Status = EventTicketStatus.Paid },
            //        new EventTicket { EventId = eventId[10].Id, UserID = userId[28].Id, PaidAmount = 220000m, QRCode = "QR142ABC", TicketCode = "679216", Status = EventTicketStatus.Paid }



            //    };

            //    await context.EventTickets.AddRangeAsync(eventTickets);
            //    await context.SaveChangesAsync();
            //}
        }
    }
}