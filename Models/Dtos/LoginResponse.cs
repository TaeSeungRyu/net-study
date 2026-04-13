namespace MemberApi.Models.Dtos
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public UserSummary User { get; set; } = new();
    }

    public class UserSummary
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? Name { get; set; }
    }
}
