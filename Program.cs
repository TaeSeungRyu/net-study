using MemberApi.Extensions;
using MemberApi.MiddleWare;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddApplicationServices();
builder.Services.AddMongo();
builder.Services.AddJwtAuth();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AuthCode}/{action=Index}/{id?}"
);

app.Run();