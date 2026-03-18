using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MemberApi.Constants;
using MemberApi.Models;

namespace MemberApi.Extensions
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddJwtAuth(this IServiceCollection services)
        {
            services
            .AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = MyJwtConstants.JwtIssuer,
                    ValidAudience = MyJwtConstants.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(MyJwtConstants.JwtSecret)
                    )
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        var response = new ApiResponse<object>(
                            false,
                            null,
                            "인증이 필요합니다."
                        );

                        await context.Response.WriteAsync(
                            System.Text.Json.JsonSerializer.Serialize(response)
                        );
                    }
                };
            });

            return services;
        }
    }
}