using BCrypt.Net;

namespace MemberApi.Security
{
    public static class PasswordUtil
    {
        public static string HashPassword(string password)
        {
            int saltRounds = 10;
            return BCrypt.Net.BCrypt.HashPassword(password, saltRounds);
        }
        public static bool ComparePassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}