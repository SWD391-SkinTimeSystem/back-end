namespace SkinTime.DTOs.Service
{
    public class ServiceDetailModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Step { get; set; }
        public int Duration { get; set; }
        public int DateToNextStep { get; set; }
    }
}
