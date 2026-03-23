using MemberApi.Extensions;
using MemberApi.MiddleWare;
using MemberApi.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDB")
);

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
app.UseAccessLog();


app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AuthCode}/{action=Index}/{id?}"
);

app.Run();