using MongoDB.Driver;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<MemberApi.Services.UserService>();
builder.Services.AddScoped<MemberApi.Services.AuthService>();
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
        ValidIssuer = "myapi",
        ValidAudience = "myapi",
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("secretkey123456")
        )
    };
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
