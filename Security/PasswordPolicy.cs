using System.Linq;
using MemberApi.Exceptions;

namespace MemberApi.Security
{
    /// <summary>
    /// 비밀번호 정책: 길이/문자 종류 검증.
    /// </summary>
    public static class PasswordPolicy
    {
        public const int MinLength = 8;
        public const int MaxLength = 100;

        public static void Validate(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ValidationException("비밀번호는 필수입니다.");

            if (password.Length < MinLength || password.Length > MaxLength)
                throw new ValidationException($"비밀번호는 {MinLength}자 이상 {MaxLength}자 이하여야 합니다.");

            if (!password.Any(char.IsLetter))
                throw new ValidationException("비밀번호는 최소 하나의 영문자를 포함해야 합니다.");

            if (!password.Any(char.IsDigit))
                throw new ValidationException("비밀번호는 최소 하나의 숫자를 포함해야 합니다.");
        }
    }
}
