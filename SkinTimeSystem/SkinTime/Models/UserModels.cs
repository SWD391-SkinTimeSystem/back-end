using SkinTime.DAL.Enum;

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

    public class AccountRegistration
        {
            public required string Username { get; set; }
            public required string Email { get; set; }
            public required string Password { get; set; }
            public required string Role { get; set; }
        }

    public class CustomerRegistration
    {
        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string Fullname { get; set; }
        
        public DateOnly DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public bool IsTermOfUseAccepted { get; set; } = false;
    }

    public class AccountInformation
    {
        public required Guid Id { get; set; }

        public required string Username { get; set; }

        public required string Email { get; set; }

        public required string Phone { get; set; }

        public required DateOnly DateOfBirth { get; set; }

        public required string Role { get; set; }

        public required DateTime CreatedTime { get; set; }

        public required DateTime LastUpdate {  get; set; }
    }

    public class UserCredential
    {
        public required string Account { get; set; }
        public required string Password { get; set; }
    }
}

