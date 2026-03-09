using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MemberApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace MemberApi.Security
{
    public class JwtTokenService
    {
        private readonly string _secret = "SUPER_SECRET_KEY_123456";
        private readonly string _issuer = "memberapi";

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id ?? ""),
                new Claim(ClaimTypes.Name, user.Username),
            };
            if (user.AuthList != null)
            {
                foreach (var auth in user.AuthList)
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