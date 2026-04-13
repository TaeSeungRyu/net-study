using System.ComponentModel.DataAnnotations;

namespace MemberApi.Models.Dtos
{
    public class CreateUserRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? ProfileImage { get; set; }

        public List<string>? Role { get; set; }
    }

    public class UpdateUserRequest
    {
        public string? Password { get; set; }

        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? ProfileImage { get; set; }

        public List<string>? Role { get; set; }
    }

    public class UserResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? ProfileImage { get; set; }
        public List<AuthCodeResponse> Roles { get; set; } = new();
    }
}
