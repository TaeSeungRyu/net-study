using System.ComponentModel.DataAnnotations;

namespace MemberApi.Config
{
    public class JwtSettings
    {
        /// <summary>
        /// HMAC-SHA256 서명 키. 최소 32바이트(=일반적으로 32자) 권장, 64자 이상 강력 권장.
        /// </summary>
        [Required]
        [MinLength(32, ErrorMessage = "Jwt:Secret 은 최소 32자 이상이어야 합니다.")]
        public string Secret { get; set; } = "";

        [Required]
        public string Issuer { get; set; } = "";

        [Required]
        public string Audience { get; set; } = "";

        [Range(1, 24 * 30)]
        public int ExpiresInHours { get; set; } = 2;
    }
}
