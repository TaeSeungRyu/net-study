using System.ComponentModel.DataAnnotations;

namespace MemberApi.Models.Dtos
{
    public class CreateAuthCodeRequest
    {
        [Required]
        public string Code { get; set; } = string.Empty;

        public string? Name { get; set; }

        public string? Desc { get; set; }
    }

    public class UpdateAuthCodeRequest
    {
        public string? Name { get; set; }

        public string? Desc { get; set; }
    }

    public class AuthCodeResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Desc { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
