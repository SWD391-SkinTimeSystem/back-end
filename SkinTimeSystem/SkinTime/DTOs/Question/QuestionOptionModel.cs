using System.Text.Json.Serialization;

namespace SkinTime.DTOs.Question
{
    public class QuestionOptionModel
    {
        public string Content { get; set; }
        public string Id { get; set; }

        [JsonPropertyName("skin_type")]
        public string SkinType { get; set; }
    }
}
