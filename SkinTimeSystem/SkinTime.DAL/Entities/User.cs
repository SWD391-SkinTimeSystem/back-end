using SkinTime.DAL.Enum;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkinTime.DAL.Entities
{
    public class User : BaseEntity
    {
        [Column(name: "fullname")]
        public string FullName { get; set; } = string.Empty;

        [Column(name: "username")]
        public string Username { get; set; } = string.Empty;

        [Column(name: "email", TypeName = "VARCHAR")]
        [MaxLength(40)]
        public required string Email { get; set; }

        [Column(name: "password", TypeName = "VARCHAR")]
        [MaxLength(250)]
        public string Password { get; set; } = string.Empty;

        [Column(name: "date_of_birth", TypeName = "DATE")]
        public DateOnly DateOfBirth { get; set; }

        [Column(name: "gender")]
        public Gender Gender { get; set; }

        [Column(name: "phone_number", TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Column(name: "role", TypeName = "VARCHAR")]
        [MaxLength(25)]
        public required UserRole Role { get; set; }

        [Column(name: "avatar", TypeName = "VARCHAR")]
        [MaxLength(255)]
        public string Avatar { get; set; } = string.Empty;

        [Column(name: "status")]
        public UserStatus Status { get; set; } = UserStatus.Inactive;

        // Navigational virtual properties represent entity relationship with other entities.
        public virtual Therapist TherapistNavigation { get; set; } = null!;
        public virtual ICollection<Booking> BookingNavigation { get; set; } = new Collection<Booking>();
        public virtual ICollection<UserChoice> UserChoices { get; set; } = new Collection<UserChoice>();
        public virtual ICollection<EventTicket> EventTickets { get; set; } = new Collection<EventTicket>();
        public virtual ICollection<Notification> Notifications { get; set; } = new Collection<Notification>();
        public virtual ICollection<Message> Messages { get; set; } = new Collection<Message>();
    }
}
