using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkinTime.DAL.Enum;

namespace SkinTime.DAL.Entities
{
    public class User : BaseEntity
    {
        [Column("fullname")]
        public string FullName { get; set; } = string.Empty;

        [Column("username")]
        public string UserName { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column(name: "password", TypeName = "VARCHAR")]
        [StringLength(250)]
        public string Password { get; set; } = string.Empty;

        [Column(name: "date_of_birth", TypeName = "DATE")]
        public DateOnly DateOfBirth { get; set; } 

        [Column(name:"gender")]
        public Gender Gender { get; set; }

        [Column(name:"phone_number", TypeName = "VARCHAR")]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Column(name: "role", TypeName = "VARCHAR(100)")]
        public string? Role { get; set; }

        public string Avatar { get; set; } = string.Empty;

        // Navigational virtual properties represent entity relationship with other entities.
        public virtual Therapist TherapistNavigation { get; set; } = null!;
        public virtual ICollection<Booking> BookingNavigation { get; set; } = new Collection<Booking>();
        public virtual ICollection<UserChoice> UserChoices { get; set; } = new Collection<UserChoice>();
        public virtual ICollection<EventTicket> EventTickets { get; set; } = new Collection<EventTicket>();
    }
}
