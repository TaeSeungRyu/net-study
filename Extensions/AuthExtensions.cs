using System.Text;
using System.Text.Json;
using MemberApi.Config;
using MemberApi.Models.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace MemberApi.Extensions
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<JwtSettings>()
                .Bind(configuration.GetSection("Jwt"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            // Bind once at startup; ValidateDataAnnotations above will throw at startup if invalid.
            var jwt = configuration.GetSection("Jwt").Get<JwtSettings>()
                ?? throw new InvalidOperationException("Jwt 설정이 없습니다.");

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwt.Issuer,
                        ValidAudience = jwt.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret)),
                        ClockSkew = TimeSpan.FromMinutes(1)
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context => WriteErrorResponse(context.Response, context.HandleResponse, 401, "인증이 필요합니다."),
                        OnForbidden = context => WriteErrorResponse(context.Response, null, 403, "접근 권한이 없습니다.")
                    };
                });

            services.AddAuthorization();
            return services;
        }

        private static async Task WriteErrorResponse(HttpResponse response, Action? handleResponse, int statusCode, string message)
        {
            handleResponse?.Invoke();
            response.StatusCode = statusCode;
            response.ContentType = "application/json";

            var body = JsonSerializer.Serialize(
                ApiResponse<object>.Fail(message),
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
            );
            await response.WriteAsync(body);
        }
    }
}
