using System.Text.Json.Serialization;

namespace SkinTime.DTOs.Question
{
    public class QuestionCreationModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonPropertyName("content")]
        public required string Content { get; set; }

        [JsonPropertyName("order_no")]
        public required int OrderNo { get; set; }

        [JsonPropertyName("choices")]
        public ICollection<QuestionChoiceCreationModel> Choices { get; set; } = new List<QuestionChoiceCreationModel>();
    }

    public class QuestionChoiceCreationModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string Content { get; set; }

        [JsonPropertyName("skin_type")]
        public required Guid SkinType { get; set; }
    }
}
