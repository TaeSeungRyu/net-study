namespace MemberApi.Security
{
    public static class PasswordUtil
    {
        private const int SaltRounds = 10;

        public static string HashPassword(string password)
            => BCrypt.Net.BCrypt.HashPassword(password, SaltRounds);

        public static bool VerifyPassword(string password, string hashedPassword)
            => BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
