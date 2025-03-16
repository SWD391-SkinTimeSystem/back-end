namespace Services.Commons.DTOs.Service
{
    public class ServiceDetailsDTO
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int Step { get; set; }
        public required int Duration { get; set; }
        public required int DateToNextStep { get; set; }
    }
}
