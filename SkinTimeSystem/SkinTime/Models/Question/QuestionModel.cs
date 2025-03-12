namespace SkinTime.Models.Question
{
    public class QuestionModel
    {
        public int No { get; set; }
        public string Content { get; set; }
        public List<QuestionOptionModel> QuestionOptions { get; set; }
        public string IdQuestion { get; set; }
    }
}
