namespace Services.Commons.DTOs.Booking
{
    public class BookingDetailDTO
    {
        public Guid Id { get; set; }

        public required string TherapistName { get; set; }
        public string Thumbnail { get; set; }
        public required string ServiceName { get; set; }
        public string Status { get; set; }

        public int TotalStep { get; set; }
        public string Description { get; set; }
        public ICollection<BookingStepDetailsDTO> Details { get; set; }

    }
}
