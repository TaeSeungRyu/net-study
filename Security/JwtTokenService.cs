using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MemberApi.Models;
using Microsoft.IdentityModel.Tokens;
using MemberApi.Constants;


namespace MemberApi.Security
{
    public class JwtTokenService
    {
        private readonly string _secret = MyJwtConstants.JwtSecret;
        private readonly string _issuer = MyJwtConstants.JwtIssuer;

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.id ?? ""),
                new Claim(ClaimTypes.Name, user.username),
            };
            if (user.role != null)
            {
                foreach (var auth in user.role)
                {
                    claims.Add(new Claim(ClaimTypes.Role, auth));
                }
                Console.WriteLine($"값: {user.username} {user.password}");
                Console.WriteLine($"값: {string.Join(", ", user.role)}");
            }
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_secret)
            );
            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _issuer,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}