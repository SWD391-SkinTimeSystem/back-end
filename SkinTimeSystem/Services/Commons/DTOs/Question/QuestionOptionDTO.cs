using System.Text.Json.Serialization;

namespace Services.Commons.DTOs.Question
{
    public class QuestionOptionDTO
    {
        public string Content { get; set; }
        public string Id { get; set; }

        [JsonPropertyName("skin_type")]
        public string SkinType { get; set; }
    }
}
