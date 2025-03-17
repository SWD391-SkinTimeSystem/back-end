namespace Services.Commons.DTOs.Question
{
    public class QuestionDTO
    {
        public int No { get; set; }
        public string Content { get; set; }
        public List<QuestionOptionDTO> QuestionOptions { get; set; }
        public string IdQuestion { get; set; }
    }
}
