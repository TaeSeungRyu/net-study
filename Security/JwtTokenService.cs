using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MemberApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace MemberApi.Security
{
    public class JwtTokenService
    {
        private readonly string _secret = "super_secret_key_for_jwt_token_123456";
        private readonly string _issuer = "memberapi";

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.id ?? ""),
                new Claim(ClaimTypes.Name, user.username),
            };
            if (user.authList != null)
            {
                foreach (var auth in user.authList)
                {
                    claims.Add(new Claim(ClaimTypes.Role, auth));
                }
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