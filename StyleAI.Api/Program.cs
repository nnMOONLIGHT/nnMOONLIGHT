using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using StyleAI.Infrastructure.Persistence;
using StyleAI.Infrastructure.Services;
using StyleAI.Core.Interfaces;
using StyleAI.Core.Data;
using System.IO;
using StyleAI.Core.Entities;
using StyleAI.Core.Enums;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// конфигурации
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);


var dbPath = Path.Combine(builder.Environment.ContentRootPath, "styleai.db");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite($"Data Source={dbPath}"));

//тоже
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

// регистр серверов
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<IStyleService, StyleService>();

// каталог для джисончикабро
var stylesJson = Path.Combine(builder.Environment.ContentRootPath, "python_ml", "styles.json");
if (File.Exists(stylesJson))
{
    builder.Services.AddSingleton<IStyleCatalog>(new StyleCatalog(stylesJson));
}

// Для контроллера мл
var pythonBase = builder.Configuration["PythonMl:BaseUrl"] ?? "http://localhost:8000";
builder.Services.AddHttpClient<IPythonMlClient, PythonMlClient>(c => c.BaseAddress = new Uri(pythonBase));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate();
    }
    catch
    {
        // миграции тестирования
        db.Database.EnsureCreated();
    }

    // Для менеджера и его логина/почты
    try
    {
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        if (!db.Users.Any(u => u.Email == "admin@local"))
        {
            db.Users.Add(new User
            {
                Email = "admin@local",
                PasswordHash = hasher.Hash("password"),
                Role = UserRole.Manager,
                CreatedAt = DateTime.UtcNow
            });
            db.SaveChanges();
        }
    }
    catch
    {
        //игнорирование ошибок
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();

app.Run();
//Ошибки не ошибки
