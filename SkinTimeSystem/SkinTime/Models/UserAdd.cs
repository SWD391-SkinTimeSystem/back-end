using System.ComponentModel.DataAnnotations;

namespace SkinTime.Models
{
    public class UserAdd
    {
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? UseName { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
    }
    public class UserAddWithRole : UserAdd
    {
        public string? Id { get; set; }
        public string? Role { get; set; }
    }
    public class UserAddTest
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
      //  public string? UseName { get; set; }
      //  public string? Password { get; set; }
       // public string? Phone { get; set; }
    }

}

