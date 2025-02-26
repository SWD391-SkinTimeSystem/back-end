using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SkinTime.DAL.Entities;

namespace SkinTime.BLL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<ServiceCategory> ServiceCategories { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;
        public DbSet<ServiceDetail> ServiceDetails { get; set; } = null!;
        public DbSet<ServiceImage> ServiceImages { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Therapist> Therapists { get; set; } = null!;
        public DbSet<TherapistCertification> TherapistCertifications {get; set; } = null!;

        public DbSet<Voucher> Vouchers { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;

        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<EventTicket> EventTickets { get; set; } = null!;

        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<Feedback> Feedbacks { get; set; } = null!;
        public DbSet<Schedule> Schedules { get; set; } = null!;
        public DbSet<Tracking> Trackings { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<SkinType> SkinTypes { get; set; } = null!;
        public DbSet<ServiceRecommendation> ServiceRecommendation { get; set; } = null!;

        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<QuestionOption> QuestionOptions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            /*
             * This is the piece of code that's manage the relationship between entity in the database.
             */

            // User - Therapist 1:1
            builder.Entity<User>()
           .HasOne(u => u.TherapistNavigation)
           .WithOne(b => b.UserNavigation)
           .HasForeignKey<Therapist>(b => b.UserID);

            // Booking - User M:1
            builder.Entity<Booking>()
                .HasOne(u => u.CustomerNavigation)
                .WithMany(b => b.BookingNavigation)
                .HasForeignKey(b => b.CustomerId);

            // Schedule - Tracking 1:1
            builder.Entity<Schedule>()
            .HasOne(u => u.TrakingNavigation)
            .WithOne(b => b.ScheduleNavigation)
            .HasForeignKey<Tracking>(b => b.ScheduleId);

            // Tracking - Therapist M:1
            builder.Entity<Tracking>()
                .HasOne(u => u.TherapistNavigation)
                .WithMany(b => b.TrackingNavigation)
                .HasForeignKey(b => b.TherapistId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // Schedule - ServiceDetail M:1
            builder.Entity<Schedule>()
                .HasOne(u => u.ServiceDetailNavigation)
                .WithMany(b => b.ScheduleNavigation)
                .HasForeignKey(b => b.ServiceDetailId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // Booking - FeedBack 1:1
            builder.Entity<Booking>()
                .HasOne(u => u.FeedbackNavigation)
                .WithOne(b => b.BookingNavigation)
                .HasForeignKey<Feedback>(b => b.BookingId)
                .OnDelete(DeleteBehavior.ClientCascade);

            /*
             * Hàm này sẽ tìm tất cả các lớp implement interface IEntityTypeConfiguration nằm trong project
             * và sử dụng nó để cấu hình các Entity.
             */
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //foreach (var entityType in builder.Model.GetEntityTypes())
            //{
            //    var tableName = entityType.GetTableName()!;
            //    if (tableName.StartsWith("AspNet"))
            //    {
            //        entityType.SetTableName(tableName.Substring(6));
            //    }
            //}

            // Hàm này gọi OnModelCreating() default của DbContext. 
            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}