using System.ComponentModel.DataAnnotations;

namespace MemberApi.Models.Dtos
{
    public class CreateUserRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? ProfileImage { get; set; }
    }

    public class UpdateUserRequest
    {
        [StringLength(100, MinimumLength = 8)]
        public string? Password { get; set; }

        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? ProfileImage { get; set; }
    }

    public class UpdateUserRolesRequest
    {
        [Required]
        public List<string> Role { get; set; } = new();
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
