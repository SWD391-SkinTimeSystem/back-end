using SkinTime.DAL.Entities;
using System.Text.Json.Serialization;

namespace SkinTime.Models
{
    public class AppointmentModel
    {
        public DateTime Date {  get; set; }
        public string Status{ get; set; }
        public string ServiceName { get; set; }
    }
    //public class User
    //{
    //    public int CustomerId { get; set; }
    //    public string Name { get; set; }

    //    [JsonIgnore] 
    //    public virtual List<BookingWithStatus>? Bookings { get; set; }
    //}
    public class BookingWithStatus
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }

        public int CustomerId { get; set; }

        [JsonIgnore] // Ngăn vòng lặp khi serialize
        public virtual User? Customer { get; set; }
    }


}
