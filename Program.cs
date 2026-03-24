using MemberApi.Extensions;
using MemberApi.MiddleWare;
using MemberApi.Config;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDB")
);

builder.Services.Configure<PostgresSettings>(
    builder.Configuration.GetSection("Postgres")
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddApplicationServices();
builder.Services.AddMongo();
builder.Services.AddPostgres();
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

// using (var scope = app.Services.CreateScope())
// {
//     var settings = scope.ServiceProvider.GetRequiredService<IOptions<PostgresSettings>>().Value;

//     try
//     {
//         using var conn = new NpgsqlConnection(settings.ConnectionString);
//         conn.Open();

//         using var cmd = new NpgsqlCommand("SELECT 1", conn);
//         var result = cmd.ExecuteScalar();

//         Console.WriteLine($"[Postgres] 앱 시작 시 연결 성공 - SELECT 1 결과: {result}");
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"[Postgres] 앱 시작 시 연결 실패: {ex.Message}");
//         throw;
//     }
// }

app.Run();
