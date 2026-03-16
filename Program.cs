using MongoDB.Driver;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MemberApi.Constants;
using MemberApi.MiddleWare;
using MemberApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<MemberApi.Services.UserService>();
builder.Services.AddScoped<MemberApi.Services.AuthService>();
builder.Services.AddScoped<MemberApi.Services.AuthCodeService>();
builder.Services.AddScoped<MemberApi.Security.JwtTokenService>();


builder.Services.AddSingleton<IMongoClient>(
    new MongoClient("mongodb://root:rootpassword@localhost:27017/appdb?authSource=admin")
);

builder.Services
.AddAuthentication("Bearer")
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Constants.JwtIssuer,
        ValidAudience = Constants.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Constants.JwtSecret)
        )
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            ApiResponse<object> response = new ApiResponse<object>(
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


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();



app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
