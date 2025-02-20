namespace SkinTime.Models
{
    public class QuestionModel
    {
        public int No { get; set; }
        public string Content { get; set; }
        public List<QuestionOptionModel> QuestionOptions { get; set; }
        public string IdQuestion { get; set; }
    }

    public class QuestionOptionModel
    {
        public string Content { get; set; }
        //public bool IsDelete { get; set; }        
        //public string SkinTypeID { get; set; }    
        //public string SkinType { get; set; }      
        //public string QuestionID { get; set; }   
        public string Id { get; set; }
    }

    public class QuestionCreationModel
    {
        public Guid? Id { get; set; } = Guid.Empty;
        public required string Content { get; set; }
        public int No { get; set; } = 0;
        public ICollection<OptionCreationModel> options { get; set; } = new List<OptionCreationModel>();
    }

    public class OptionCreationModel
    {
        public Guid? Id { get; set; } = Guid.Empty;
        public required string Content { get; set; }
        public required Guid SkinType { get; set; }
    }
}
